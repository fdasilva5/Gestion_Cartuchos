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
  Collapse,
  Paper,
  IconButton,
  MenuItem,
  Select,
  FormControl,
  InputLabel,
  Container,
  Grid,
  Divider,
  Link,
} from '@mui/material';
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import ExpandLessIcon from '@mui/icons-material/ExpandLess';
import AddIcon from '@mui/icons-material/Add';
import colorConfigs from '../../configs/colorConfigs'; // Asegúrate de ajustar la ruta según tu estructura de archivos
import ModalNuevoCartucho from '../../components/modals/ModalNuevoCartucho'; // Asegúrate de ajustar la ruta según tu estructura de archivos

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

const ListaCartuchosPage = () => {
  const [cartuchos, setCartuchos] = useState<Cartucho[]>([]);
  const [modelos, setModelos] = useState<Modelo[]>([]);
  const [estados, setEstados] = useState<Estado[]>([]);
  const [numeroSerieBuscado, setNumeroSerieBuscado] = useState('');
  const [modeloFiltro, setModeloFiltro] = useState('');
  const [estadoFiltro, setEstadoFiltro] = useState('');
  const [cartuchosExpandidos, setCartuchosExpandidos] = useState<{ [key: number]: boolean }>({});
  const [openModal, setOpenModal] = useState(false);
  const [selectedCartucho, setSelectedCartucho] = useState<Cartucho | undefined>(undefined);
  const [loading, setLoading] = useState(false); // Estado de carga

  const API_URL = "http://localhost:5204";

  const fetchCartuchos = async () => {
    try {
      const response = await fetch(`${API_URL}/api/Cartucho`);
      const data = await response.json();
      setCartuchos(data);
    } catch (error) {
      console.error('Error fetching data:', error);
    }
  };

  const fetchModelos = async () => {
    try {
      const response = await fetch(`${API_URL}/api/Modelo`);
      const data = await response.json();
      setModelos(data);
    } catch (error) {
      console.error('Error fetching modelos:', error);
    }
  };

  const fetchEstados = async () => {
    try {
      const response = await fetch(`${API_URL}/api/Estado`);
      const data = await response.json();
      setEstados(data);
    } catch (error) {
      console.error('Error fetching estados:', error);
    }
  };

  useEffect(() => {
    fetchCartuchos();
    fetchModelos();
    fetchEstados();
  }, []);

  const cartuchosFiltrados = cartuchos.filter(cartucho =>
    cartucho.numero_serie.toLowerCase().includes(numeroSerieBuscado.toLowerCase()) &&
    (modeloFiltro ? cartucho.modelo.modelo_cartuchos === modeloFiltro : true) &&
    (estadoFiltro ? cartucho.estado.nombre === estadoFiltro : true)
  );

  const toggleCartucho = (id: number) => {
    setCartuchosExpandidos(prevState => ({
      ...prevState,
      [id]: !prevState[id],
    }));
  };

  const handleAddCartucho = () => {
    setSelectedCartucho(undefined);
    setOpenModal(true);
  };

  const handleSaveCartucho = async () => {
    await fetchCartuchos(); // Recargar la lista de cartuchos
    setOpenModal(false); // Cerrar modal si se guardó correctamente
  };

  const handleRecargarCartucho = async (cartuchoId: number) => {
    const recargaData = {
      id: 0,
      fecha_recarga: new Date().toISOString().split('T')[0], // Default to current date
      observaciones: "Recarga realizada",
      cartucho_id: cartuchoId,
      cartucho: {
        id: 0,
        numero_serie: "string",
        fecha_alta: new Date().toISOString().split('T')[0],
        cantidad_recargas: 0,
        observaciones: "string",
        modelo_id: 0,
        modelo: {
          id: 0,
          modelo_cartuchos: "string",
          marca: "string",
          stock: 0
        },
        estado_id: 0,
        estado: {
          id: 0,
          nombre: "string"
        }
      }
    };

    try {
      await fetch(`${API_URL}/api/Recarga`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          'accept': 'text/plain',
        },
        body: JSON.stringify(recargaData),
      });
      fetchCartuchos(); // Refresh data to show updated stock
    } catch (error) {
      console.error('Error recargando cartucho:', error);
    }
  };

  const handleEstadoEnRecarga = async (cartuchoId: number) => {
    try {
      await fetch(`${API_URL}/api/Cartucho/estadoEnRecarga/${cartuchoId}`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
          'accept': '*/*',
        },
      });
      fetchCartuchos(); // Refresh data to show updated estado
    } catch (error) {
      console.error('Error cambiando estado del cartucho:', error);
    }
  };

  const handleDesasignarCartucho = async (cartuchoId: number) => {
    try {
      await fetch(`${API_URL}/api/Asignar_Impresora/desasignar?cartuchoId=${cartuchoId}`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          'accept': '*/*',
        },
        body: '',
      });
      fetchCartuchos(); // Refresh data to show updated estado
    } catch (error) {
      console.error('Error desasignando cartucho:', error);
    }
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
        Lista de Cartuchos
      </Typography>
      <br />
      <Divider />
      <br />
      <Box display="flex" justifyContent="center">
        <Button
          variant="contained"
          color="primary"
          startIcon={<AddIcon />}
          onClick={handleAddCartucho}
          sx={{ backgroundColor: 'green', color: 'white' }}
        >
          Añadir Cartucho
        </Button>
      </Box>
      <br />
      <br />
      <Divider />

      <Paper elevation={5} sx={{ padding: '2rem', marginBottom: '2rem' }}>
        <Typography variant="h5" gutterBottom>
          Aplicar Filtros
        </Typography>
        <Grid container spacing={2} alignItems="center">
          <Grid item xs={12} md={4}>
            <TextField
              variant="outlined"
              fullWidth
              placeholder="Buscar por número de serie"
              value={numeroSerieBuscado}
              onChange={(e) => setNumeroSerieBuscado(e.target.value)}
              size="small"
              sx={{ backgroundColor: 'white', borderRadius: 1 }}
            />
          </Grid>
          <Grid item xs={12} md={4}>
            <FormControl variant="outlined" size="small" fullWidth sx={{ backgroundColor: 'white', borderRadius: 1 }}>
              <InputLabel>Modelo</InputLabel>
              <Select
                value={modeloFiltro}
                onChange={(e) => setModeloFiltro(e.target.value)}
                label="Modelo"
              >
                <MenuItem value=""><em>Todos</em></MenuItem>
                {modelos.map(modelo => (
                  <MenuItem key={modelo.id} value={modelo.modelo_cartuchos}>{modelo.modelo_cartuchos}</MenuItem>
                ))}
              </Select>
            </FormControl>
          </Grid>
          <Grid item xs={12} md={4}>
            <FormControl variant="outlined" size="small" fullWidth sx={{ backgroundColor: 'white', borderRadius: 1 }}>
              <InputLabel>Estado</InputLabel>
              <Select
                value={estadoFiltro}
                onChange={(e) => setEstadoFiltro(e.target.value)}
                label="Estado"
              >
                <MenuItem value=""><em>Todos</em></MenuItem>
                {estados.map(estado => (
                  <MenuItem key={estado.id} value={estado.nombre} sx={{ color: estado.id === 1 ? 'green' : estado.id === 2 ? 'blue' : estado.id === 3 ? 'orange' : estado.id === 4 ? 'red' : estado.id === 5 ? 'purple' : 'inherit' }}>
                    {estado.nombre}
                  </MenuItem>
                ))}
              </Select>
            </FormControl>
          </Grid>
        </Grid>
      </Paper>

      <Paper elevation={5} sx={{ padding: '2rem' }}>
        <TableContainer component={Paper} sx={{ maxHeight: 400 }}>
          <Table stickyHeader>
            <TableHead>
              <TableRow>
                <TableCell>Detalles</TableCell>
                <TableCell>Número de Serie</TableCell>
                <TableCell align='center'>Cantidad de Recargas</TableCell>
                <TableCell>Modelo</TableCell>
                <TableCell>Marca</TableCell>
                <TableCell>Estado</TableCell>
                <TableCell>Acciones</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {cartuchosFiltrados.length > 0 ? (
                cartuchosFiltrados.map(cartucho => (
                  <React.Fragment key={cartucho.id}>
                    <TableRow hover sx={{ backgroundColor: cartuchosExpandidos[cartucho.id] ? '#f9f9f9' : 'white' }}>
                      <TableCell>
                        <IconButton size="small" onClick={() => toggleCartucho(cartucho.id)}>
                          {cartuchosExpandidos[cartucho.id] ? <ExpandLessIcon /> : <ExpandMoreIcon />}
                        </IconButton>
                      </TableCell>
                      <TableCell>{cartucho.numero_serie}</TableCell>
                     <TableCell align='center'>{cartucho.cantidad_recargas}</TableCell>
                      <TableCell sx={{ color: 'blue', fontWeight: 'bold' }}>{cartucho.modelo.modelo_cartuchos}</TableCell>
                      <TableCell>{cartucho.modelo.marca}</TableCell>
                      <TableCell  sx={{ color: cartucho.estado.nombre === 'Disponible' ? 'green' : cartucho.estado.nombre === 'En impresora' ? '#da0000' : cartucho.estado.nombre === 'Esperando recarga' ? '#ff7300' : cartucho.estado.nombre === 'Dado de baja' ? 'red' : cartucho.estado.nombre === 'En recarga' ? 'purple' : 'inherit' }}>
                        {cartucho.estado.nombre}
                      </TableCell>
                      <TableCell>
                        {cartucho.estado_id === 1 ? (
                          <Link href={`asignar`} underline="none">
                            <Button variant="contained" color="primary" sx={{ backgroundColor: 'blue', color: 'white' }}>
                              Asignar Cartucho
                            </Button>
                          </Link>
                        ) : cartucho.estado_id === 2 ? (
                          <Button
                            variant="contained"
                            color="secondary"
                            onClick={() => handleDesasignarCartucho(cartucho.id)}
                            sx={{ backgroundColor: '#da0000', color: 'white' }}
                          >
                            Desasignar
                          </Button>
                        ) : cartucho.estado_id === 3 ? (
                          <Button
                            variant="contained"
                            color="primary"
                            onClick={() => handleEstadoEnRecarga(cartucho.id)}
                            sx={{ backgroundColor: 'orange', color: 'white' }}
                          >
                            En recarga
                          </Button>
                        ) : (
                          <Button
                            variant="contained"
                            color="secondary"
                            onClick={() => handleRecargarCartucho(cartucho.id)}
                            sx={{ backgroundColor: 'purple', color: 'white' }}
                          >
                            Recargado
                          </Button>
                        )}
                      </TableCell>
                    </TableRow>
                    <TableRow>
                      <TableCell style={{ paddingBottom: 0, paddingTop: 0 }} colSpan={8}>
                        <Collapse in={cartuchosExpandidos[cartucho.id]} timeout="auto" unmountOnExit>
                          <Box margin={1} sx={{ backgroundColor: '#f9f9f9', padding: 2 }}>
                            <Typography variant="h6" gutterBottom component="div">
                              Detalles del Cartucho
                            </Typography>
                            <Typography>Observaciones: {cartucho.observaciones}</Typography>
                            <Typography>Fecha de Alta {cartucho.fecha_alta}</Typography>
                          </Box>
                        </Collapse>
                      </TableCell>
                    </TableRow>
                  </React.Fragment>
                ))
              ) : (
                <TableRow>
                  <TableCell colSpan={8} align="center">
                    <Typography>No se encontraron cartuchos.</Typography>
                  </TableCell>
                </TableRow>
              )}
            </TableBody>
          </Table>
        </TableContainer>
      </Paper>

      <ModalNuevoCartucho
        open={openModal}
        onClose={() => setOpenModal(false)}
        onSave={handleSaveCartucho}
        cartucho={selectedCartucho}
      />
    </Container>
  );
};

export default ListaCartuchosPage;