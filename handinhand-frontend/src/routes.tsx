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
import Profile from './pages/Profile';
import MySkills from './pages/MySkills';
import MyRequests from './pages/MyRequests';
import SkillDetail from './pages/SkillDetail';
import RequestDetail from './pages/RequestDetail';

export const router = createBrowserRouter([
  {
    element: <Layout />,
    children: [
      { index: true, element: <Home /> },
      { path: 'skills', element: <Skills /> },
      { path: 'requests', element: <Requests /> },
      /* 详情页（动态参数 :id） */
      { path: 'skills/:id', element: <SkillDetail /> },
      { path: 'requests/:id', element: <RequestDetail /> },
      /* 登录注册相关 */
      { path: 'login', element: <Login /> },
      { path: 'register', element: <Register /> },
      /* 需要登录的页面用 PrivateRoute 包起来 */
      {
        path: 'profile',
        element: (
          <PrivateRoute>
            <Profile />
          </PrivateRoute>
        ),
      },
      {
        path: 'my/skills',
        element: (
          <PrivateRoute>
            <MySkills />
          </PrivateRoute>
        ),
      },
      {
        path: 'my/requests',
        element: (
          <PrivateRoute>
            <MyRequests />
          </PrivateRoute>
        ),
      },
      /* 新建技能和求助 */
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
