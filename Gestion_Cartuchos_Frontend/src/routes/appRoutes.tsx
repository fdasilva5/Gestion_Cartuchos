import HomePage from "../pages/home/HomePage";
import { RouteType } from "./config";
import CartuchosPage from "../pages/cartuchos/cartuchosPageLayout";
import ChangelogPage from "../pages/changelog/ChangelogPage";
import DashboardOutlinedIcon from '@mui/icons-material/DashboardOutlined';
import AppsOutlinedIcon from '@mui/icons-material/AppsOutlined';
import ArticleOutlinedIcon from '@mui/icons-material/ArticleOutlined';
import FormatListBulletedOutlinedIcon from '@mui/icons-material/FormatListBulletedOutlined';
import AlertPage from "../pages/impresoras/AlertPage";
import ButtonPage from "../pages/impresoras/ButtonPage";
import AsignarCartuchosPage from "../pages/cartuchos/AsignarCartuchosPage";
import ListaCartuchosPage from "../pages/cartuchos/ListaCartuchosPage";
import RecargasPageLayout from "../pages/recargas/RecargasPageLayout";
import ImpresoraPageLayout from "../pages/impresoras/ImpresoraPageLayout";



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
        path: "/impresoras/button",
        element: <ButtonPage />,
        state: "impresoras.button",
        sidebarProps: {
          displayText: "Button"
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
    }
  },
  {
    path: "/changelog",
    element: <ChangelogPage />,
    state: "changelog",
    sidebarProps: {
      displayText: "Changelog",
      icon: <FormatListBulletedOutlinedIcon />
    }
  }
];

export default appRoutes;