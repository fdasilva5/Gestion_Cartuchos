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
  Checkbox,
  ListItemText,
  SelectChangeEvent,
} from "@mui/material";
import CloseIcon from "@mui/icons-material/Close";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { faComment } from "@fortawesome/free-solid-svg-icons";
import { show_alerta } from '../../helpers/funcionSweetAlert';

const ModalNuevaImpresora = ({
  open,
  onClose,
  onSave,
  impresora,
}: {
  open: boolean;
  onClose: () => void;
  onSave: (impresora: any) => void;
  impresora?: any;
}) => {
  const [modelo, setModelo] = useState("");
  const [marca, setMarca] = useState("");
  const [observaciones, setObservaciones] = useState("");
  const [oficinaId, setOficinaId] = useState<number | string>("");
  const [modelosCartuchoSeleccionados, setModelosCartuchoSeleccionados] = useState<number[]>([]);
  const [modelosCartucho, setModelosCartucho] = useState<any[]>([]);
  const [oficinas, setOficinas] = useState<any[]>([]);
  const [mensaje, setMensaje] = useState("");
  const [loading, setLoading] = useState(false);
  // Estados de error
  const [modeloError, setModeloError] = useState("");
  const [marcaError, setMarcaError] = useState("");
  const [oficinaError, setOficinaError] = useState("");

  const API_URL = "http://localhost:5204";

  useEffect(() => {
    if (impresora) {
      setModelo(impresora.modelo);
      setMarca(impresora.marca);
      setObservaciones(impresora.observaciones);
      setOficinaId(impresora.oficina_id);
      setModelosCartuchoSeleccionados(impresora.impresoraModelos.map((c: any) => c.modelo.id));
    } else {
      resetForm();
    }
  }, [impresora]);

  useEffect(() => {
    const fetchModelosCartucho = async () => {
      try {
        const response = await fetch(`${API_URL}/api/Modelo`);
        const data = await response.json();
        setModelosCartucho(data);
      } catch (error) {
        console.error('Error fetching modelos cartucho:', error);
      }
    };
    fetchModelosCartucho();
  }, []);

  useEffect(() => {
    const fetchOficinas = async () => {
      try {
        const response = await fetch(`${API_URL}/api/Oficina`);
        const data = await response.json();
        setOficinas(data);
      } catch (error) {
        console.error('Error fetching oficinas:', error);
      }
    };
    fetchOficinas();
  }, []);

  // Resetear el formulario
  const resetForm = () => {
    setModelo("");
    setMarca("");
    setObservaciones("");
    setOficinaId("");
    setModelosCartuchoSeleccionados([]);
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
    if (!modelo) {
      setModeloError("Este campo es requerido.");
      valid = false;
    } else {
      setModeloError("");
    }

    if (!marca) {
      setMarcaError("Este campo es requerido.");
      valid = false;
    } else {
      setMarcaError("");
    }

    if (!oficinaId) {
      setOficinaError("Este campo es requerido.");
      valid = false;
    } else {
      setOficinaError("");
    }

    if (modelosCartuchoSeleccionados.length === 0) {
      setMensaje("Selecciona al menos un modelo de cartucho compatible.");
      valid = false;
    } else {
      setMensaje("");
    }

    if (!valid) return;

    const oficina = oficinas.find((o) => o.id === oficinaId);

    const impresoraDTO = {
      id: impresora?.id || 0,
      modelo: modelo,
      marca: marca,
      oficina_id: oficinaId,
      oficina: {
        id: oficina.id,
        nombre: oficina.nombre
      },
      compatibleModeloIds: modelosCartuchoSeleccionados,
    };

    setLoading(true);

    try {
      const method = impresora?.id ? "PUT" : "POST";
      const url = impresora?.id
        ? `${API_URL}/api/Impresora/${impresora.id}`
        : `${API_URL}/api/Impresora`;
      const response = await fetch(url, {
        method,
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(impresoraDTO),
      });

      if (response.ok) {
        const data = await response.json();
        show_alerta(impresora?.id ? "Impresora actualizada con éxito" : "Impresora guardada con éxito", "success");
        onSave(impresoraDTO);
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

  const handleChangeCartuchos = (event: SelectChangeEvent<number[]>) => {
    setModelosCartuchoSeleccionados(event.target.value as number[]);
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
          <Typography variant="h6">{impresora ? "Editar Impresora" : "Añadir Nueva Impresora"}</Typography>
          <IconButton onClick={onClose}>
            <CloseIcon />
          </IconButton>
        </Box>
        <form onSubmit={handleSubmit}>
          <TextField
            fullWidth
            label="Modelo"
            value={modelo}
            onChange={(e) => setModelo(e.target.value)}
            sx={{ mb: 2 }}
            error={Boolean(modeloError)}
            helperText={modeloError}
          />
          <TextField
            fullWidth
            label="Marca"
            value={marca}
            onChange={(e) => setMarca(e.target.value)}
            sx={{ mb: 2 }}
            error={Boolean(marcaError)}
            helperText={marcaError}
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
          <FormControl fullWidth sx={{ mb: 2 }} error={Boolean(oficinaError)}>
            <InputLabel>Oficina</InputLabel>
            <Select
              value={oficinaId}
              onChange={(e) => setOficinaId(Number(e.target.value))}
              label="Oficina"
            >
              {oficinas.map((oficina) => (
                <MenuItem key={oficina.id} value={oficina.id}>
                  {oficina.nombre}
                </MenuItem>
              ))}
            </Select>
            <FormHelperText>{oficinaError}</FormHelperText>
          </FormControl>
          <FormControl fullWidth sx={{ mb: 2 }} error={Boolean(mensaje)}>
            <InputLabel>Modelos de Cartuchos Compatibles</InputLabel>
            <Select
              multiple
              value={modelosCartuchoSeleccionados}
              onChange={handleChangeCartuchos}
              renderValue={(selected) => (selected as number[]).map(id => modelosCartucho.find(c => c.id === id)?.modelo_cartuchos).join(', ')}
            >
              {modelosCartucho.map((modelo) => (
                <MenuItem key={modelo.id} value={modelo.id}>
                  <Checkbox checked={modelosCartuchoSeleccionados.indexOf(modelo.id) > -1} />
                  <ListItemText primary={`${modelo.modelo_cartuchos} - ${modelo.marca}`} />
                </MenuItem>
              ))}
            </Select>
            <FormHelperText>{mensaje}</FormHelperText>
          </FormControl>
          <Button type="submit" variant="contained" color="primary" disabled={loading}>
            {loading ? 'Guardando...' : 'Guardar'}
          </Button>
        </form>
      </Box>
    </Modal>
  );
};

export default ModalNuevaImpresora;