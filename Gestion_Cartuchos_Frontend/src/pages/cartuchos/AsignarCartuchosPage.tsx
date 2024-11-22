import React, { useEffect, useState } from 'react';
import {
  Box,
  Button,
  TextField,
  MenuItem,
  Select,
  FormControl,
  InputLabel,
  Container,
  Typography,
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

type ModeloCartuchoCompatible = {
  id: number;
  modelo_cartuchos: string;
  marca: string;
  stock: number;
};

type Oficina = {
  id: number;
  nombre: string;
};

type Cartucho = {
  id: number;
  numero_serie: string;
  fecha_alta: string;
  cantidad_recargas: number;
  observaciones: string;
  modelo: ModeloCartuchoCompatible;
  modelo_id: number;
  estado_id: number;
  estado: {
    id: number;
    nombre: string;
  };
};

type Impresora = {
  id: number;
  modelo: string;
  marca: string;
  oficina_id: number;
  oficina: Oficina;
  modelo_cartucho_compatible: ModeloCartuchoCompatible[];
  cartucho_asignado?: Cartucho;
};

type Assignment = {
  id: number;
  impresora_id: number;
  impresora: Impresora;
  oficina_id: number;
  oficina:Oficina;
  cartucho_id: number;
  cartucho: Cartucho;
  fecha_asignacion: string;
  fecha_desasignacion: string | null;
  observaciones: string | null;
};

const AsignarCartuchosPage = () => {
  const [impresoras, setImpresoras] = useState<Impresora[]>([]);
  const [cartuchos, setCartuchos] = useState<Cartucho[]>([]);
  const [assignments, setAssignments] = useState<Assignment[]>([]);
  const [selectedImpresora, setSelectedImpresora] = useState<number | ''>('');
  const [selectedCartucho, setSelectedCartucho] = useState<number | ''>('');
  const [observaciones, setObservaciones] = useState('');
  const [impresoraFilter, setImpresoraFilter] = useState('');
  const [cartuchoFilter, setCartuchoFilter] = useState('');

  const API_URL = "http://localhost:5204";

  const fetchData = async () => {
    try {
      const [impresorasResponse, cartuchosResponse, assignmentsResponse] = await Promise.all([
        fetch(`${API_URL}/api/Impresora`),
        fetch(`${API_URL}/api/Cartucho/disponibles`),
        fetch(`${API_URL}/api/Asignar_Impresora`)
      ]);

      const impresorasData = await impresorasResponse.json();
      const cartuchosData = await cartuchosResponse.json();
      const assignmentsData = await assignmentsResponse.json();

      const impresorasWithAssignments = impresorasData.map((impresora: Impresora) => {
        const assignment = assignmentsData.find((a: any) => a.impresora_id === impresora.id);
        if (assignment) {
          impresora.cartucho_asignado = assignment.cartucho;
        }
        return impresora;
      });

      setImpresoras(impresorasWithAssignments);
      setCartuchos(cartuchosData);
      setAssignments(assignmentsData);
    } catch (error) {
      console.error('Error fetching data:', error);
    }
  };

  useEffect(() => {
    fetchData();
  }, []);

  const handleSaveAssignment = async () => {
    if (selectedImpresora && selectedCartucho) {
      const impresora = impresoras.find(i => i.id === selectedImpresora);
      const cartucho = cartuchos.find(c => c.id === selectedCartucho);

      if (!impresora || !cartucho) {
        console.error('Impresora or Cartucho not found');
        return;
      }

      const assignment = {
        id: 0,
        impresora_id: selectedImpresora,
        impresora: {
          id: impresora.id,
          modelo: impresora.modelo,
          marca: impresora.marca,
          oficina: impresora.oficina,
          oficina_id: impresora.oficina_id,
          modelos_cartucho_compatibles: impresora.modelo_cartucho_compatible,
        },
        cartucho_id: selectedCartucho,
        cartucho: {
          id: cartucho.id,
          numero_serie: cartucho.numero_serie,
          fecha_alta: cartucho.fecha_alta,
          cantidad_recargas: cartucho.cantidad_recargas,
          observaciones: cartucho.observaciones,
          modelo: cartucho.modelo,
          modelo_id: cartucho.modelo_id,
          estado_id: 1, // Ensure estado_id is set to 1
          estado: {
            id: 1,
            nombre: 'Disponible', // Ensure estado is set correctly
          },
        },
        fecha_asignacion: new Date().toISOString().split('T')[0], // Default to current date
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
          body: JSON.stringify(assignment), // Include assignment field
        });
        // Reset form fields after successful submission
        setSelectedImpresora('');
        setSelectedCartucho('');
        setObservaciones('');
        fetchData(); // Refresh data to show updated assignments
      } catch (error) {
        console.error('Error assigning cartucho:', error);
      }
    }
  };

  const handleImpresoraChange = async (impresoraId: number | '') => {
    setSelectedImpresora(impresoraId);
    if (impresoraId) {
      try {
        const response = await fetch(`${API_URL}/api/Cartucho/impresora/${impresoraId}`);
        const compatibleCartuchos = await response.json();
        setCartuchos(compatibleCartuchos);
      } catch (error) {
        console.error('Error fetching compatible cartuchos:', error);
      }
    } else {
      setCartuchos([]);
    }
    const impresora = impresoras.find(i => i.id === impresoraId);
    if (impresora && impresora.cartucho_asignado) {
      setSelectedCartucho(impresora.cartucho_asignado.id);
    } else {
      setSelectedCartucho('');
    }
  };

  const handleDeassignCartucho = async (cartuchoId: number) => {
    try {
      await fetch(`${API_URL}/api/Asignar_Impresora/desasignar?cartuchoId=${cartuchoId}`, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
          'accept': 'text/plain',
        },
      });
      fetchData(); // Refresh data to show updated assignments
    } catch (error) {
      console.error('Error deassigning cartucho:', error);
    }
  };

  const filteredImpresoras = impresoras.filter(impresora =>
    impresora.modelo.toLowerCase().includes(impresoraFilter.toLowerCase()) ||
    impresora.marca.toLowerCase().includes(impresoraFilter.toLowerCase()) ||
    impresora.oficina?.nombre.toLowerCase().includes(impresoraFilter.toLowerCase())
  );

  const filteredCartuchos = cartuchos.filter(cartucho =>
    cartucho.numero_serie.toLowerCase().includes(cartuchoFilter.toLowerCase()) ||
    cartucho.modelo.modelo_cartuchos.toLowerCase().includes(cartuchoFilter.toLowerCase()) ||
    cartucho.modelo.marca.toLowerCase().includes(cartuchoFilter.toLowerCase())
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
        Asignar Cartuchos a Impresoras
      </Typography>
      <Paper elevation={5} sx={{ padding: '2rem', marginBottom: '2rem' }}>
        <Grid container spacing={2}>
          <Grid item xs={12} md={6}>
            <TextField
              variant="outlined"
              fullWidth
              label="Filtrar Impresoras"
              value={impresoraFilter}
              onChange={(e) => setImpresoraFilter(e.target.value)}
              sx={{ marginBottom: 2 }}
            />
            <FormControl variant="outlined" fullWidth sx={{ marginBottom: 2 }}>
              <InputLabel>Impresora</InputLabel>
              <Select
                value={selectedImpresora}
                onChange={(e) => handleImpresoraChange(e.target.value as number | '')}
                label="Impresora"
              >
                <MenuItem value=""><em>None</em></MenuItem>
                {filteredImpresoras.map(impresora => (
                  <MenuItem key={impresora.id} value={impresora.id}>
                    {impresora.modelo} - {impresora.marca} ({impresora.oficina?.nombre})
                  </MenuItem>
                ))}
              </Select>
            </FormControl>
          </Grid>
          <Grid item xs={12} md={6}>
            <TextField
              variant="outlined"
              fullWidth
              label="Filtrar Cartuchos"
              value={cartuchoFilter}
              onChange={(e) => setCartuchoFilter(e.target.value)}
              sx={{ marginBottom: 2 }}
            />
            <FormControl variant="outlined" fullWidth sx={{ marginBottom: 2 }}>
              <InputLabel>Cartucho</InputLabel>
              <Select
                value={selectedCartucho}
                onChange={(e) => setSelectedCartucho(e.target.value as number | '')}
                label="Cartucho"
              >
                <MenuItem value=""><em>None</em></MenuItem>
                {impresoras.find(i => i.id === selectedImpresora)?.cartucho_asignado && (
                  <MenuItem value={impresoras.find(i => i.id === selectedImpresora)?.cartucho_asignado?.id}>
                    {impresoras.find(i => i.id === selectedImpresora)?.cartucho_asignado?.numero_serie} - {impresoras.find(i => i.id === selectedImpresora)?.cartucho_asignado?.modelo.modelo_cartuchos} ({impresoras.find(i => i.id === selectedImpresora)?.cartucho_asignado?.modelo.marca})
                  </MenuItem>
                )}
                {filteredCartuchos.map(cartucho => (
                  <MenuItem key={cartucho.id} value={cartucho.id}>
                    {cartucho.numero_serie} - {cartucho.modelo.modelo_cartuchos} ({cartucho.modelo.marca})
                  </MenuItem>
                ))}
              </Select>
            </FormControl>
          </Grid>
          <Grid item xs={12}>
            <TextField
              variant="outlined"
              fullWidth
              label="Observaciones"
              value={observaciones}
              onChange={(e) => setObservaciones(e.target.value)}
              multiline
              rows={4}
              sx={{ marginBottom: 2 }}
            />
          </Grid>
          <Grid item xs={12}>
            <Box display="flex" justifyContent="center">
              <Button
                variant="contained"
                color="primary"
                onClick={handleSaveAssignment}
                sx={{ backgroundColor: 'green', color: 'white' }}
              >
                Asignar Cartucho
              </Button>
            </Box>
          </Grid>
        </Grid>
      </Paper>
      <Paper elevation={5} sx={{ padding: '2rem', marginBottom: '2rem' }}>
        <Typography variant="h5" gutterBottom>
          Cartuchos Asignados
        </Typography>
        <TableContainer component={Paper}>
          <Table>
            <TableHead>
              <TableRow>
                <TableCell>Impresora</TableCell>
                <TableCell>Oficina</TableCell>
                <TableCell>Cartucho</TableCell>
                <TableCell>Fecha de Asignación</TableCell>
                <TableCell>Observaciones</TableCell>
                <TableCell>Acciones</TableCell>
              </TableRow>
            </TableHead>
            <TableBody>
              {assignments.map(assignment => (
                <TableRow key={assignment.id}>
                  <TableCell>{assignment.impresora.modelo} - {assignment.impresora.marca} </TableCell>
                  <TableCell>{assignment.impresora.oficina?.nombre} </TableCell>
                  <TableCell>{assignment.cartucho.numero_serie} - {assignment.cartucho.modelo.modelo_cartuchos} ({assignment.cartucho.modelo.marca})</TableCell>
                  <TableCell>{assignment.fecha_asignacion}</TableCell>
                  <TableCell>{assignment.observaciones || 'N/A'}</TableCell>
                  <TableCell>
                    <Button
                      variant="contained"
                      color="secondary"
                      onClick={() => handleDeassignCartucho(assignment.cartucho_id)}
                      sx={{ backgroundColor: '#da0000', color: 'white' }}
                    >
                      Desasignar
                    </Button>
                  </TableCell>
                </TableRow>
              ))}
            </TableBody>
          </Table>
        </TableContainer>
      </Paper>
    </Container>
  );
};

export default AsignarCartuchosPage;