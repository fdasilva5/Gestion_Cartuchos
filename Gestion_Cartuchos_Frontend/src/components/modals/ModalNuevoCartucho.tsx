import React, { useState, useEffect } from 'react';
import {
  Box,
  Button,
  TextField,
  Typography,
  Modal,
  IconButton,
  InputAdornment,
  FormControl,
  InputLabel,
  Select,
  MenuItem,
  FormHelperText,
  Checkbox,
  ListItemText,
  SelectChangeEvent,
} from '@mui/material';
import CloseIcon from '@mui/icons-material/Close';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faComment } from '@fortawesome/free-solid-svg-icons';
import { show_alerta } from '../../helpers/funcionSweetAlert';

const ModalNuevoCartucho = ({
  open,
  onClose,
  onSave,
  cartucho,
}: {
  open: boolean;
  onClose: () => void;
  onSave: (cartucho: any) => void;
  cartucho?: any;
}) => {
  const [numeroSerie, setNumeroSerie] = useState('');
  const [modelo, setModelo] = useState<any>(null);
  const [observaciones, setObservaciones] = useState('');
  const [modelos, setModelos] = useState<any[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');

  const API_URL = "http://localhost:5204";

  useEffect(() => {
    if (cartucho) {
      setNumeroSerie(cartucho.numero_serie);
      setModelo(cartucho.modelo);
      setObservaciones(cartucho.observaciones);
    } else {
      resetForm();
    }
  }, [cartucho]);

  useEffect(() => {
    const fetchModelos = async () => {
      try {
        const response = await fetch(`${API_URL}/api/Modelo`);
        const data = await response.json();
        setModelos(data);
      } catch (error) {
        console.error('Error fetching modelos:', error);
      }
    };
    fetchModelos();
  }, []);

  const resetForm = () => {
    setNumeroSerie('');
    setModelo(null);
    setObservaciones('');
    setError('');
  };

  useEffect(() => {
    if (!open) {
      resetForm();
    }
  }, [open]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    const cartuchoDTO = {
      id: cartucho?.id || 0,
      numero_serie: numeroSerie,
      fecha_alta: new Date().toISOString().split('T')[0], // Assuming current date as fecha_alta
      cantidad_recargas: 0, // Assuming 0 as cantidad_recargas
      observaciones,
      modelo_id: modelo?.id || 0,
      estado_id: 1,
      modelo: modelo ? { id: modelo.id, marca: modelo.marca, modelo_cartuchos: modelo.modelo_cartuchos, stock: modelo.stock } : null,
      estado: { id: 1, nombre: "Disponible" },
    };

    setLoading(true);

    try {
      const method = cartucho?.id ? "PUT" : "POST";
      const url = cartucho?.id ? `${API_URL}/api/Cartucho/${cartucho.id}` : `${API_URL}/api/Cartucho`;
      const response = await fetch(url, {
        method,
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(cartuchoDTO),
      });

      if (response.ok) {
        const data = await response.json();
        show_alerta(cartucho?.id ? "Cartucho actualizado con éxito" : "Cartucho guardado con éxito", "success");

        onSave(cartuchoDTO);
        onClose();
      } else {
        const errorData = await response.json();
        if (errorData.message.includes("El número de serie ya existe.")) {
          setError("El número de serie ya existe.");
        } else {
          setError(errorData.message || response.statusText);
        }
        console.error(`Error: ${errorData.message || response.statusText}`);
      }
    } catch (error) {
      console.error("Error en la solicitud:", error);
      setError("Ocurrió un error inesperado.");
      show_alerta("Ocurrió un error inesperado.", "error");
    } finally {
      setLoading(false);
    }
  };

  return (
    <Modal open={open} onClose={onClose}>
      <Box
        sx={{
          position: "absolute",
          top: "50%",
          left: "50%",
          transform: "translate(-50%, -50%)",
          width: 600,
          bgcolor: "background.paper",
          boxShadow: 24,
          p: 4,
        }}
      >
        <Box display="flex" justifyContent="space-between" alignItems="center">
          <Typography variant="h6">{cartucho ? "Editar Cartucho" : "Añadir Nuevo Cartucho"}</Typography>
          <IconButton onClick={onClose}>
            <CloseIcon />
          </IconButton>
        </Box>
        <form onSubmit={handleSubmit}>
          <TextField
            fullWidth
            label="Número de Serie"
            value={numeroSerie}
            onChange={(e) => setNumeroSerie(e.target.value)}
            sx={{ mb: 2 }}
            error={Boolean(error)}
            helperText={error}
          />
          <FormControl fullWidth sx={{ mb: 2 }}>
            <InputLabel>Modelo</InputLabel>
            <Select
              value={modelo?.id || ''}
              onChange={(e) => setModelo(modelos.find(m => m.id === e.target.value))}
              label="Modelo"
            >
              {modelos.map((modelo) => (
                <MenuItem key={modelo.id} value={modelo.id}>
                  {modelo.modelo_cartuchos} - {modelo.marca}
                </MenuItem>
              ))}
            </Select>
          </FormControl>
          <TextField
            fullWidth
            label="Observaciones"
            value={observaciones}
            onChange={(e) => setObservaciones(e.target.value)}
            sx={{ mb: 2 }}
            InputProps={{
              startAdornment: (
                <InputAdornment position="start">
                  <FontAwesomeIcon icon={faComment} />
                </InputAdornment>
              ),
            }}
          />
          <Button type="submit" variant="contained" color="primary" disabled={loading}>
            {loading ? 'Guardando...' : 'Guardar'}
          </Button>
        </form>
      </Box>
    </Modal>
  );
};

export default ModalNuevoCartucho;