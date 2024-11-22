import React, { useState, useEffect } from "react";
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
} from "@mui/material";
import CloseIcon from "@mui/icons-material/Close";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faBarcode, faCalendar, faComment } from "@fortawesome/free-solid-svg-icons";
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
  const [numeroSerie, setNumeroSerie] = useState("");
  const [fechaAlta, setFechaAlta] = useState("");
  const [observaciones, setObservaciones] = useState("");
  const [modeloId, setModeloId] = useState(0);
  const [modelos, setModelos] = useState<any[]>([]);
  const [mensaje, setMensaje] = useState("");
  const [loading, setLoading] = useState(false);
  // Estados de error
  const [numeroSerieError, setNumeroSerieError] = useState("");
  const [fechaAltaError, setFechaAltaError] = useState("");
  const [modeloIdError, setModeloIdError] = useState("");

  const API_URL = "http://localhost:5204";

  useEffect(() => {
    if (cartucho) {
      setNumeroSerie(cartucho.numero_serie);
      setFechaAlta(cartucho.fecha_alta);
      setObservaciones(cartucho.observaciones);
      setModeloId(cartucho.modelo_id);
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

  // Resetear el formulario
  const resetForm = () => {
    setNumeroSerie("");
    setFechaAlta(new Date().toISOString().split('T')[0]); // Fecha actual
    setObservaciones("");
    setModeloId(0);
    setMensaje("");
  };

  useEffect(() => {
    if (!open) {
      resetForm();
    }
  }, [open]);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();

    
    // Validaciones
    let valid = true;
    if (!numeroSerie) {
      setNumeroSerieError("Este campo es requerido.");
      valid = false;
    } else {
      setNumeroSerieError("");
    }

    if (!fechaAlta) {
      setFechaAltaError("Este campo es requerido.");
      valid = false;
    } else {
      setFechaAltaError("");
    }

    if (!modeloId) {
      setModeloIdError("Selecciona un modelo.");
      valid = false;
    } else {
      setModeloIdError("");
    }

    if (!valid) return; 

    const modelo = modelos.find((m) => m.id === modeloId);

    const cartuchoDTO = {
      numero_serie: numeroSerie,
      fecha_alta: fechaAlta,
      cantidad_recargas: 0,
      observaciones: observaciones,
      modelo_id: modeloId,
      estado_id: 1,
      modelo: modelo ? { id: modelo.id, marca: modelo.marca,modelo_cartuchos: modelo.modelo_cartuchos, stock: modelo.stock } : null,
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
        console.error(`Error: ${errorData.message || response.statusText}`);
      }
    } catch (error) {
      console.error("Error en la solicitud:", error);
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
            InputProps={{
              startAdornment: (
                <InputAdornment position="start">
                  <FontAwesomeIcon icon={faBarcode} />
                </InputAdornment>
              ),
            }}
            error={Boolean(numeroSerieError)}
            helperText={numeroSerieError}
          />
          <TextField
            fullWidth
            label="Fecha de Alta"
            type="date"
            value={fechaAlta}
            onChange={(e) => setFechaAlta(e.target.value)}
            sx={{ mb: 2 }}
            InputLabelProps={{ shrink: true }}
            error={Boolean(fechaAltaError)}
            helperText={fechaAltaError}
          />
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
          <FormControl fullWidth sx={{ mb: 2 }} error={Boolean(modeloIdError)}>
            <InputLabel>Modelo</InputLabel>
            <Select value={modeloId} onChange={(e) => setModeloId(Number(e.target.value))}>
              <MenuItem value={0}>Seleccionar Modelo</MenuItem>
              {modelos.map((modelo) => (
                <MenuItem key={modelo.id} value={modelo.id}>{modelo.marca}</MenuItem>
              ))}
            </Select>
            <FormHelperText>{modeloIdError}</FormHelperText>
          </FormControl>
          <Button type="submit" variant="contained" color="primary" disabled={loading}>
            {loading ? 'Guardando...' : 'Guardar'}
          </Button>
        </form>
      </Box>
    </Modal>
  );
};

export default ModalNuevoCartucho;
