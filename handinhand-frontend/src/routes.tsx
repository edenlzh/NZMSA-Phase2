import { createBrowserRouter } from 'react-router-dom';
import Layout from './Layout';
import Home from './pages/Home';
import Skills from './pages/Skills';
import Requests from './pages/Requests';
import Login from './pages/Login';
import Register from './pages/Register';
import SkillForm from './pages/SkillForm';
import RequestForm from './pages/RequestForm';
import PrivateRoute from './components/PrivateRoute';

export const router = createBrowserRouter([
  {
    element: <Layout />,
    children: [
      { index: true, element: <Home /> },
      { path: 'skills', element: <Skills /> },
      { path: 'requests', element: <Requests /> },
      { path: 'login', element: <Login /> },
      { path: 'register', element: <Register /> },
      {
        path: 'skills/new',
        element: (
          <PrivateRoute>
            <SkillForm />
          </PrivateRoute>
        ),
      },
      {
        path: 'requests/new',
        element: (
          <PrivateRoute>
            <RequestForm />
          </PrivateRoute>
        ),
      },
    ],
  },
]);
