import { Routes, Route } from 'react-router-dom';
import './assets/styles/App.css';
import Navbar from './components/Navbar/Navbar';
import HomePage from './pages/HomePage';
import Footer from './components/Footer/Footer';
import ShopPage from './pages/ShopPage';
import SubscribePage from './pages/SubscribePage';
import RulesPage from './pages/RulesPage';
import NotFoundPage from './pages/NotFoundPage';
import PublicPage from './pages/Server/PublicPage';
import AuthCallback from './pages/Callback';
import ProfilePage from './pages/ProfilePage';
import AdminHomePage from './pages/Admin/AdminHomePage';
import AdminUsersPage from './pages/Admin/AdminUsersPage';
import AdminLogsPage from './pages/Admin/AdminLogsPage';

export default function App() {
  return (
    <>
        <div className='min-h-screen flex flex-col'>
          <Navbar/>

          <main className='min-w-0 flex-1'>
            <Routes>
              <Route path='/' element = { <HomePage /> }/>
              <Route path='/shop' element = { <ShopPage /> }/>
              <Route path='/subscribtion' element = { <SubscribePage /> }/>
              <Route path='/rules' element = { <RulesPage /> }/>
              <Route path='*' element = { <NotFoundPage /> }/>
              <Route path='/servers/public' element = { <PublicPage /> } />
              <Route path='/auth/callback' element = { <AuthCallback /> } />
              <Route path="/profile/:steamId" element={ <ProfilePage /> } />
              <Route path='/admin' element = { <AdminHomePage /> } />
              <Route path='/admin/users' element = { <AdminUsersPage /> } />
              <Route path='/admin/logs' element = { <AdminLogsPage /> } />
            </Routes>
          </main>

          <Footer />
        </div>
    </>
  )
}

