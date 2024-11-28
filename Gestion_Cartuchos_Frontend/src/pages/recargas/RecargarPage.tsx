import React, { useEffect, useState } from 'react';
import {
  Box,
  Button,
  TextField,
  Typography,
  Container,
  Grid,
  Paper,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
} from '@mui/material';
import colorConfigs from '../../configs/colorConfigs'; // Ajusta la ruta según sea necesario

type Modelo = {
  id: number;
  modelo_cartuchos: string;
  marca: string;
  stock: number;
};

type Estado = {
  id: number;
  nombre: string;
};

type Cartucho = {
  id: number;
  numero_serie: string;
  fecha_alta: string;
  cantidad_recargas: number;
  observaciones: string;
  modelo_id: number;
  modelo: Modelo;
  estado_id: number;
  estado: Estado;
};

type Recarga = {
  id: number;
  fecha_recarga: string;
  observaciones: string;
  cartucho_id: number;
  cartucho: Cartucho;
};

const RecargarPage = () => {
  const [recargas, setRecargas] = useState<Recarga[]>([]);
  const [filter, setFilter] = useState('');

  const API_URL = "http://localhost:5204";

  const fetchRecargas = async () => {
    try {
      const response = await fetch(`${API_URL}/api/Recarga`);
      const data = await response.json();
      setRecargas(data);
    } catch (error) {
      console.error('Error fetching recargas:', error);
    }
  };

  useEffect(() => {
    fetchRecargas();
  }, []);

  const filteredRecargas = recargas.filter(recarga =>
    recarga.cartucho.numero_serie.toLowerCase().includes(filter.toLowerCase()) ||
    recarga.cartucho.modelo.modelo_cartuchos.toLowerCase().includes(filter.toLowerCase()) ||
    recarga.cartucho.modelo.marca.toLowerCase().includes(filter.toLowerCase())
  );

  return (
    <Container maxWidth="lg">
      <Typography
        variant="h4"
        gutterBottom
        align="center"
        sx={{
          fontWeight: 'bold',
          backgroundColor: colorConfigs.sidebar.bg,
          color: colorConfigs.sidebar.color,
          padding: '1rem',
          borderRadius: '8px',
          boxShadow: 3,
          marginBottom: '2rem',
        }}
      >
        Lista de Recargas
      </Typography>
    <Paper elevation={5} sx={{ padding: '2rem', marginBottom: '2rem' }}>
      <Grid container spacing={2} alignItems="center">
        <Grid item xs={12} md={6}>
        <TextField
          variant="outlined"
          fullWidth
          label="Filtrar por Número de Serie"
          value={filter}
          onChange={(e) => setFilter(e.target.value)}
          sx={{ marginBottom: 2 }}
        />
        </Grid>
        </Grid>
      </Paper>
      <Paper elevation={5} sx={{ padding: '2rem' }}>
        <TableContainer component={Paper} sx={{ maxHeight: 400 }}>
          <Table stickyHeader>
            <TableHead>
              <TableRow>
                <TableCell>Fecha de Recarga</TableCell>
                <TableCell>Número de Serie</TableCell>
                <TableCell>Modelo</TableCell>
                <TableCell>Marca</TableCell>
                <TableCell>Estado</TableCell>
                <TableCell>Observaciones</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {filteredRecargas.length > 0 ? (
                filteredRecargas.map(recarga => (
                  <TableRow key={recarga.id}>
                    <TableCell>{recarga.fecha_recarga}</TableCell>
                    <TableCell>{recarga.cartucho.numero_serie}</TableCell>
                    <TableCell>{recarga.cartucho.modelo.modelo_cartuchos}</TableCell>
                    <TableCell>{recarga.cartucho.modelo.marca}</TableCell>
                    <TableCell>{recarga.cartucho.estado.nombre}</TableCell>
                    <TableCell>{recarga.observaciones}</TableCell>
                  </TableRow>
                ))
              ) : (
                <TableRow>
                  <TableCell colSpan={6} align="center">
                    <Typography>No se encontraron recargas.</Typography>
                  </TableCell>
                </TableRow>
              )}
            </TableBody>
          </Table>
        </TableContainer>
      </Paper>
    </Container>
  );
};

export default RecargarPage;