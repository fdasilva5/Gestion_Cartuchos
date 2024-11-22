import React, { useState } from "react";
import { useEffect } from "react";

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
import { show_alerta } from '../../helpers/funcionSweetAlert';

const ModalModeloCartucho = ({
  open,
  onClose,
  onSave,
}: {
  open: boolean;
  onClose: () => void;
  onSave: () => void;
}) => {
  const [modeloCartuchos, setModeloCartuchos] = useState("");
  const [marca, setMarca] = useState("");
  const [stock, setStock] = useState(0);
  const [loading, setLoading] = useState(false);
  // Estados de error
  const [modeloCartuchosError, setModeloCartuchosError] = useState("");
  const [marcaError, setMarcaError] = useState("");
  const [stockError, setStockError] = useState("");

  const API_URL = "http://localhost:5204";

  const resetForm = () => {
    setModeloCartuchos("");
    setMarca("");
    setStock(0);
    setModeloCartuchosError("");
    setMarcaError("");
    setStockError("");
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
    if (!modeloCartuchos) {
      setModeloCartuchosError("Este campo es requerido.");
      valid = false;
    } else {
      setModeloCartuchosError("");
    }

    if (!marca) {
      setMarcaError("Este campo es requerido.");
      valid = false;
    } else {
      setMarcaError("");
    }

    if (stock < 0) {
      setStockError("El stock no puede ser negativo.");
      valid = false;
    } else {
      setStockError("");
    }

    if (!valid) return;

    const modeloCartuchoDTO = {
      id: 0,
      modelo_cartuchos: modeloCartuchos,
      marca: marca,
      stock: stock,
    };

    setLoading(true);

    try {
      const response = await fetch(`${API_URL}/api/Modelo`, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(modeloCartuchoDTO),
      });

      if (response.ok) {
        const data = await response.json();
        show_alerta("Modelo de cartucho guardado con éxito", "success");
        onSave();
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
          <Typography variant="h6">Añadir Nuevo Modelo de Cartucho</Typography>
          <IconButton onClick={onClose}>
            <CloseIcon />
          </IconButton>
        </Box>
        <form onSubmit={handleSubmit}>
          <TextField
            fullWidth
            label="Modelo de Cartuchos"
            value={modeloCartuchos}
            onChange={(e) => setModeloCartuchos(e.target.value)}
            sx={{ mb: 2 }}
            error={Boolean(modeloCartuchosError)}
            helperText={modeloCartuchosError}
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
            label="Stock"
            type="number"
            value={stock}
            onChange={(e) => setStock(Number(e.target.value))}
            sx={{ mb: 2 }}
            error={Boolean(stockError)}
            helperText={stockError}
          />
          <Button type="submit" variant="contained" color="primary" disabled={loading}>
            {loading ? 'Guardando...' : 'Guardar'}
          </Button>
        </form>
      </Box>
    </Modal>
  );
};

export default ModalModeloCartucho;
