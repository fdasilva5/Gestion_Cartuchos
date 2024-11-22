import React, { useEffect, useState } from 'react';
import {
  Box,
  Button,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Typography,
  Paper,
  Container,
  Divider,
} from '@mui/material';
import AddIcon from '@mui/icons-material/Add';
import colorConfigs from '../../configs/colorConfigs'; // Ajusta la ruta según sea necesario
import ModalNuevaOficina from '../../components/modals/ModalNuevaOficina'; // Ajusta la ruta según sea necesario

type Oficina = {
  id: number;
  nombre: string;
};

const ListarOficinasPage = () => {
  const [oficinas, setOficinas] = useState<Oficina[]>([]);
  const [openModal, setOpenModal] = useState(false);
  const [loading, setLoading] = useState(false);

  const API_URL = "http://localhost:5204";

  const fetchOficinas = async () => {
    try {
      const response = await fetch(`${API_URL}/api/Oficina`);
      const data = await response.json();
      setOficinas(data);
    } catch (error) {
      console.error('Error fetching data:', error);
    }
  };

  useEffect(() => {
    fetchOficinas();
  }, []);

  const handleAddOficina = () => {
    setOpenModal(true);
  };

  const handleSaveOficina = async () => {
    await fetchOficinas(); // Recargar la lista de oficinas
    setOpenModal(false); // Cerrar modal si se guardó correctamente
  };

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
        Lista de Oficinas
      </Typography>
      <br />
      <Divider />
      <br />
      <Box display="flex" justifyContent="center">
        <Button
          variant="contained"
          color="primary"
          startIcon={<AddIcon />}
          onClick={handleAddOficina}
          sx={{ backgroundColor: 'green', color: 'white' }}
        >
          Añadir Oficina
        </Button>
      </Box>
      <br />
      <br />
      <Divider />

      <Paper elevation={5} sx={{ padding: '2rem' }}>
        <TableContainer component={Paper} sx={{ maxHeight: 400 }}>
          <Table stickyHeader>
            <TableHead>
              <TableRow>
                <TableCell>ID</TableCell>
                <TableCell>Nombre</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {oficinas.length > 0 ? (
                oficinas.map(oficina => (
                  <TableRow key={oficina.id}>
                    <TableCell>{oficina.id}</TableCell>
                    <TableCell>{oficina.nombre}</TableCell>
                  </TableRow>
                ))
              ) : (
                <TableRow>
                  <TableCell colSpan={2} align="center">
                    <Typography>No se encontraron oficinas.</Typography>
                  </TableCell>
                </TableRow>
              )}
            </TableBody>
          </Table>
        </TableContainer>
      </Paper>

      <ModalNuevaOficina
        open={openModal}
        onClose={() => setOpenModal(false)}
        onSave={handleSaveOficina}
      />
    </Container>
  );
};

export default ListarOficinasPage;