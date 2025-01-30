import React from 'react';
import { useNavigate } from 'react-router-dom';
import '../css/ArtistHome.css';

function ArtistHome() {
  const navigate = useNavigate();

  const handleLogout = () => {
    // Có thể thêm logic xử lý logout ở đây (ví dụ: xóa token, clear localStorage, etc.)
    navigate('/');
  };

  return (
    <div className="artist-home-container">
      <nav className="artist-nav">
        <h1>Artist Dashboard</h1>
        <div className="nav-links">
          <a href="#profile">Profile</a>
          <a href="#works">My Works</a>
          <a href="#settings">Settings</a>
          <a onClick={handleLogout} className="logout-btn" style={{cursor: 'pointer'}}>Logout</a>
        </div>
      </nav>
      
      <main className="artist-main">
        <div className="welcome-section">
          <h2>Welcome, Artist!</h2>
          <p>Manage your artworks and profile here</p>
        </div>
        
        <div className="dashboard-cards">
          <div className="card">
            <h3>Total Works</h3>
            <p>0</p>
          </div>
          <div className="card">
            <h3>Views</h3>
            <p>0</p>
          </div>
          <div className="card">
            <h3>Likes</h3>
            <p>0</p>
          </div>
        </div>
      </main>
    </div>
  );
}

export default ArtistHome;
