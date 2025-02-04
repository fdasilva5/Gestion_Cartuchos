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
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
} from '@mui/material';
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import ExpandLessIcon from '@mui/icons-material/ExpandLess';
import AddIcon from '@mui/icons-material/Add';
import colorConfigs from '../../configs/colorConfigs'; // Ajusta la ruta según sea necesario
import ModalNuevaImpresora from '../../components/modals/ModalNuevaImpresora'; // Ajusta la ruta según sea necesario
import { useNavigate } from 'react-router-dom';

type Modelo = {
  id: number;
  modelo_cartuchos: string;
  marca: string;
  stock: number;
};

type Oficina = {
  id: number;
  nombre: string;
};

type ImpresoraModelo = {
  id: number;
  impresora_id: number;
  modelo_id: number;
  modelo: Modelo;
};

type Impresora = {
  id: number;
  modelo: string;
  marca: string;
  oficina_id: number;
  oficina: Oficina;
  impresora_modelos: ImpresoraModelo[];
};

const ListaImpresorasPage = () => {
  const [impresoras, setImpresoras] = useState<Impresora[]>([]);
  const [numeroSerieBuscado, setNumeroSerieBuscado] = useState('');
  const [modeloFiltro, setModeloFiltro] = useState('');
  const [oficinaFiltro, setOficinaFiltro] = useState('');
  const [impresorasExpandidas, setImpresorasExpandidas] = useState<{ [key: number]: boolean }>({});
  const [openModal, setOpenModal] = useState(false);
  const [selectedImpresora, setSelectedImpresora] = useState<Impresora | undefined>(undefined);
  const [loading, setLoading] = useState(false);
  const [openAssignModal, setOpenAssignModal] = useState(false);
  const [selectedCartucho, setSelectedCartucho] = useState<number | undefined>(undefined);
  const [observaciones, setObservaciones] = useState('');
  const [compatibleCartuchos, setCompatibleCartuchos] = useState<Modelo[]>([]);

  const API_URL = "http://localhost:5204";

  const navigate = useNavigate();
  
  const fetchImpresoras = async () => {
    try {
      const response = await fetch(`${API_URL}/api/Impresora`);
      const data = await response.json();
      setImpresoras(data);
    } catch (error) {
      console.error('Error fetching data:', error);
    }
  };

  useEffect(() => {
    fetchImpresoras();
  }, []);

  const impresorasFiltradas = impresoras.filter(impresora =>
    impresora.modelo.toLowerCase().includes(numeroSerieBuscado.toLowerCase()) &&
    (modeloFiltro ? impresora.modelo === modeloFiltro : true) &&
    (oficinaFiltro ? impresora.oficina.nombre === oficinaFiltro : true)
  );

  const toggleImpresora = async (id: number) => {
    setImpresorasExpandidas(prevState => ({
      ...prevState,
      [id]: !prevState[id],
    }));

    if (!impresorasExpandidas[id]) {
      try {
        const response = await fetch(`${API_URL}/api/Impresora/modelos?impresoraId=${id}`);
        const compatibleCartuchosData = await response.json();
        setCompatibleCartuchos(compatibleCartuchosData);
      } catch (error) {
        console.error('Error fetching compatible cartuchos:', error);
      }
    }
  };

  const handleAddImpresora = () => {
    setSelectedImpresora(undefined);
    setOpenModal(true);
  };

  const handleSaveImpresora = async () => {
    await fetchImpresoras(); // Recargar la lista de impresoras
    setOpenModal(false); // Cerrar modal si se guardó correctamente
  };

  const handleSaveAssignment = async () => {
    if (selectedImpresora && selectedCartucho) {
      const assignment = {
        id: 0,
        impresora_id: selectedImpresora.id,
        cartucho_id: selectedCartucho,
        fecha_asignacion: new Date().toISOString(),
        fecha_desasignacion: null,
        observaciones,
      };

      try {
        await fetch(`${API_URL}/api/Asignar_Impresora`, {
          method: 'POST',
          headers: {
            'Content-Type': 'application/json',
            'accept': 'text/plain',
          },
          body: JSON.stringify(assignment),
        });
        setOpenAssignModal(false);
        setSelectedCartucho(undefined);
        setObservaciones('');
      } catch (error) {
        console.error('Error assigning cartucho:', error);
      }
    }
  };

  const handleAssignCartucho = (impresora: Impresora) => {
    // Navega a la página específica de asignación
    navigate(`/cartuchos/asignar`);
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
        Lista de Impresoras
      </Typography>
      <br />
      <Divider />
      <br />
      <Box display="flex" justifyContent="center">
        <Button
          variant="contained"
          color="primary"
          startIcon={<AddIcon />}
          onClick={handleAddImpresora}
          sx={{ backgroundColor: 'green', color: 'white' }}
        >
          Añadir Impresora
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
              placeholder="Buscar por modelo de impresora"
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
                {impresoras.map(impresora => (
                  <MenuItem key={impresora.id} value={impresora.modelo}>{impresora.modelo}</MenuItem>
                ))}
              </Select>
            </FormControl>
          </Grid>
          <Grid item xs={12} md={4}>
            <FormControl variant="outlined" size="small" fullWidth sx={{ backgroundColor: 'white', borderRadius: 1 }}>
              <InputLabel>Oficina</InputLabel>
              <Select
                value={oficinaFiltro}
                onChange={(e) => setOficinaFiltro(e.target.value)}
                label="Oficina"
              >
                <MenuItem value=""><em>Todas</em></MenuItem>
                {impresoras.map(impresora => (
                  <MenuItem key={impresora.oficina.id} value={impresora.oficina.nombre}>{impresora.oficina.nombre}</MenuItem>
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
                <TableCell>Modelo</TableCell>
                <TableCell>Marca</TableCell>
                <TableCell>Oficina</TableCell>
                <TableCell>Acciones</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {impresorasFiltradas.length > 0 ? (
                impresorasFiltradas.map(impresora => (
                  <React.Fragment key={impresora.id}>
                    <TableRow hover sx={{ backgroundColor: impresorasExpandidas[impresora.id] ? '#f9f9f9' : 'white' }}>
                      <TableCell>
                        <IconButton size="small" onClick={() => toggleImpresora(impresora.id)}>
                          {impresorasExpandidas[impresora.id] ? <ExpandLessIcon /> : <ExpandMoreIcon />}
                        </IconButton>
                      </TableCell>
                      <TableCell>{impresora.modelo}</TableCell>
                      <TableCell>{impresora.marca}</TableCell>
                      <TableCell>{impresora.oficina.nombre}</TableCell>
                      <TableCell>
                            <Button
                              variant="contained"
                              color="primary"
                              onClick={() => handleAssignCartucho(impresora)}
                            >
                              Asignar Cartucho
                            </Button>
                      </TableCell>
                    </TableRow>
                    <TableRow>
                      <TableCell style={{ paddingBottom: 0, paddingTop: 0 }} colSpan={5}>
                        <Collapse in={impresorasExpandidas[impresora.id]} timeout="auto" unmountOnExit>
                          <Box margin={1} sx={{ backgroundColor: '#f9f9f9', padding: 2 }}>
                            <Typography variant="h6" gutterBottom component="div">
                              Modelos de cartuchos compatibles con esta impresora
                            </Typography>
                            <ul>
                              {compatibleCartuchos.map(cartucho => (
                                <li key={cartucho.id}>
                                  {cartucho.modelo_cartuchos} - {cartucho.marca} (Stock: {cartucho.stock})
                                </li>
                              ))}
                            </ul>
                          </Box>
                        </Collapse>
                      </TableCell>
                    </TableRow>
                  </React.Fragment>
                ))
              ) : (
                <TableRow>
                  <TableCell colSpan={5} align="center">
                    <Typography>No se encontraron impresoras.</Typography>
                  </TableCell>
                </TableRow>
              )}
            </TableBody>
          </Table>
        </TableContainer>
      </Paper>

      <ModalNuevaImpresora
        open={openModal}
        onClose={() => setOpenModal(false)}
        onSave={handleSaveImpresora}
        impresora={selectedImpresora}
      />

      <Dialog open={openAssignModal} onClose={() => setOpenAssignModal(false)}>
        <DialogTitle>Asignar Cartucho</DialogTitle>
        <DialogContent>
          <FormControl variant="outlined" fullWidth sx={{ marginBottom: 2 }}>
            <InputLabel>Cartucho</InputLabel>
            <Select
              value={selectedCartucho}
              onChange={(e) => setSelectedCartucho(Number(e.target.value))}
              label="Cartucho"
            >
              {compatibleCartuchos.map(cartucho => (
                <MenuItem key={cartucho.id} value={cartucho.id}>
                  {cartucho.modelo_cartuchos} - {cartucho.marca}
                </MenuItem>
              ))}
            </Select>
          </FormControl>
          <TextField
            variant="outlined"
            fullWidth
            label="Observaciones"
            value={observaciones}
            onChange={(e) => setObservaciones(e.target.value)}
            multiline
            rows={4}
          />
        </DialogContent>
        <DialogActions>
          <Button onClick={() => setOpenAssignModal(false)} color="secondary">
            Cancelar
          </Button>
          <Button onClick={handleSaveAssignment} color="primary">
            Asignar
          </Button>
        </DialogActions>
      </Dialog>
    </Container>
  );
};

export default ListaImpresorasPage;