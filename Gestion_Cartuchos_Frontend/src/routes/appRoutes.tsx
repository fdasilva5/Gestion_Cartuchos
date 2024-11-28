import HomePage from "../pages/home/HomePage";
import { RouteType } from "./config";
import CartuchosPage from "../pages/cartuchos/cartuchosPageLayout";
import ChangelogPage from "../pages/changelog/ChangelogPage";
import DashboardOutlinedIcon from '@mui/icons-material/DashboardOutlined';
import AppsOutlinedIcon from '@mui/icons-material/AppsOutlined';
import ArticleOutlinedIcon from '@mui/icons-material/ArticleOutlined';
import FormatListBulletedOutlinedIcon from '@mui/icons-material/FormatListBulletedOutlined';

import AsignarCartuchosPage from "../pages/cartuchos/AsignarCartuchosPage";
import ListaCartuchosPage from "../pages/cartuchos/ListaCartuchosPage";
import RecargasPageLayout from "../pages/recargas/RecargasPageLayout";
import ImpresoraPageLayout from "../pages/impresoras/ImpresoraPageLayout";
import ListaImpresorasPage from "../pages/impresoras/ListarImpresoraPage";
import ListarModelosCartuchosPage from "../pages/cartuchos/ListarModelosCartuchosPage";
import ListarOficinasPage from "../pages/oficinas/ListarOficinasPage";
import RecargarPage from "../pages/recargas/RecargarPage";



const appRoutes: RouteType[] = [
  {
    index: true,
    element: <HomePage />,
    state: "home"
  },
  {
    path: "/cartuchos",
    element: <CartuchosPage />,
    state: "cartuchos",
    sidebarProps: {
      displayText: "Cartuchos",
      icon: <DashboardOutlinedIcon />
    },
    child: [
      
      {
        path: "/cartuchos/asignar",
        element: <AsignarCartuchosPage />,
        state: "cartuchos.asignar",
        sidebarProps: {
          displayText: "Asignar Cartuchos"
        }
      },
      {
        path: "/cartuchos/lista",
        element: <ListaCartuchosPage />,
        state: "cartuchos.lista",
        sidebarProps: {
          displayText: "Lista de Cartuchos"
        }
      },
      {
        path: "/cartuchos/modelos",
        element: <ListarModelosCartuchosPage />,
        state: "cartuchos.modelos",
        sidebarProps: {
          displayText: "Modelos de Cartuchos"
        }
      }
    ]
  },
  {
    path: "/impresoras",
    element: <ImpresoraPageLayout />,
    state: "impresoras",
    sidebarProps: {
      displayText: "Impresoras",
      icon: <AppsOutlinedIcon />
    },
    child: [
      {
        path: "/impresoras/lista",
        element: <ListaImpresorasPage />,
        state: "impresoras.lista",
        sidebarProps: {
          displayText: "Lista de Impresoras"
        }
      }
    ]
  },
  {
    path: "/recargas",
    element: <RecargasPageLayout />,
    state: "recargas",
    sidebarProps: {
      displayText: "Recargas",
      icon: <ArticleOutlinedIcon />
    },
    child: [
      {
        path: "/recargas/historial",
        element: <RecargarPage />,
        state: "recargas.historial",
        sidebarProps: {
          displayText: "Historial de recargas"
        }
      }
    ]
  },
  {
    path: "/oficinas",
    element: <ListarOficinasPage />,
    state: "oficinas",
    sidebarProps: {
      displayText: "Oficinas",
      icon: <FormatListBulletedOutlinedIcon />
    }
  }
];

export default appRoutes;