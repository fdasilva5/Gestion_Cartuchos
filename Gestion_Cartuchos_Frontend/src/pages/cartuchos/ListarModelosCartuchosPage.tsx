import React, { useEffect, useState } from 'react';
import {
  Box,
  Button,
  TextField,
  Table,
  TableBody,
  TableCell,
  TableContainer,
  TableHead,
  TableRow,
  Typography,
  Paper,
  IconButton,
  Container,
  Divider,
} from '@mui/material';
import AddIcon from '@mui/icons-material/Add';
import colorConfigs from '../../configs/colorConfigs'; // Ajusta la ruta según sea necesario
import ModalModeloCartucho from '../../components/modals/ModalModeloCartucho'; // Ajusta la ruta según sea necesario

type ModeloCartucho = {
  id: number;
  modelo_cartuchos: string;
  marca: string;
  stock: number;
};

const ListarModelosCartuchosPage = () => {
  const [modelosCartucho, setModelosCartucho] = useState<ModeloCartucho[]>([]);
  const [openModal, setOpenModal] = useState(false);
  const [loading, setLoading] = useState(false);

  const API_URL = "http://localhost:5204";

  const fetchModelosCartucho = async () => {
    try {
      const response = await fetch(`${API_URL}/api/Modelo`);
      const data = await response.json();
      setModelosCartucho(data);
    } catch (error) {
      console.error('Error fetching data:', error);
    }
  };

  useEffect(() => {
    fetchModelosCartucho();
  }, []);

  const handleAddModeloCartucho = () => {
    setOpenModal(true);
  };

  const handleSaveModeloCartucho = async () => {
    await fetchModelosCartucho(); // Recargar la lista de modelos de cartuchos
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
        Lista de Modelos de Cartuchos
      </Typography>
      <br />
      <Divider />
      <br />
      <Box display="flex" justifyContent="center">
        <Button
          variant="contained"
          color="primary"
          startIcon={<AddIcon />}
          onClick={handleAddModeloCartucho}
          sx={{ backgroundColor: 'green', color: 'white' }}
        >
          Añadir Modelo de Cartucho
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
                <TableCell>Modelo de Cartuchos</TableCell>
                <TableCell>Marca</TableCell>
                <TableCell>Stock</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {modelosCartucho.length > 0 ? (
                modelosCartucho.map(modelo => (
                  <TableRow key={modelo.id}>
                    <TableCell>{modelo.modelo_cartuchos}</TableCell>
                    <TableCell>{modelo.marca}</TableCell>
                    <TableCell>{modelo.stock}</TableCell>
                  </TableRow>
                ))
              ) : (
                <TableRow>
                  <TableCell colSpan={4} align="center">
                    <Typography>No se encontraron modelos de cartuchos.</Typography>
                  </TableCell>
                </TableRow>
              )}
            </TableBody>
          </Table>
        </TableContainer>
      </Paper>

      <ModalModeloCartucho
        open={openModal}
        onClose={() => setOpenModal(false)}
        onSave={handleSaveModeloCartucho}
      />
    </Container>
  );
};

export default ListarModelosCartuchosPage;