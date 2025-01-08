import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import '../css/Store.css';

function Store() {
  const [showSidebar, setShowSidebar] = useState(false);
  const [products, setProducts] = useState([]);
  const [newProduct, setNewProduct] = useState({
    name: '',
    price: '',
    category: '',
    quantity: '',
    description: ''
  });
  const navigate = useNavigate();

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setNewProduct(prev => ({
      ...prev,
      [name]: value
    }));
  };

  const handleSubmit = (e) => {
    e.preventDefault();
    setProducts([...products, { ...newProduct, id: Date.now() }]);
    setNewProduct({
      name: '',
      price: '',
      category: '',
      quantity: '',
      description: ''
    });
  };

  const handleDelete = (id) => {
    setProducts(products.filter(product => product.id !== id));
  };

  const handleLogout = () => navigate('/');
  const handleHome = () => navigate('/home');
  const handleArtist = () => navigate('/artist');
  const handleWaitlist = () => navigate('/waitlist');

  return (
    <div className="store-container">
      {/* Sidebar */}
      <div 
        className={`sidebar ${showSidebar ? 'expanded' : ''}`}
        onMouseEnter={() => setShowSidebar(true)}
        onMouseLeave={() => setShowSidebar(false)}
      >
        <div className="logo-container">
          <h1 className="logo">{showSidebar ? 'INBS' : 'IB'}</h1>
        </div>

        <div className="sidebar-buttons">
          <button className="sidebar-button" onClick={handleHome}>
            <span className="button-icon" style={{marginRight: "12px", marginLeft: "-12px", fontSize: "20px"}}>🏠</span>
            {showSidebar && 'Home'}
          </button>

          <button className="sidebar-button" onClick={handleArtist}>
            <span className="button-icon" style={{marginRight: "12px", marginLeft: "-12px", fontSize: "20px"}}>🎨</span>
            {showSidebar && 'Artist'}
          </button>

          <button className="sidebar-button" onClick={handleWaitlist}>
            <span className="button-icon" style={{marginRight: "12px", marginLeft: "-12px", fontSize: "20px"}}>⏳</span>
            {showSidebar && 'Waitlist'}
          </button>

          <button className="sidebar-button" onClick={handleLogout}>
            <span className="button-icon" style={{marginRight: "12px", marginLeft: "-12px", fontSize: "20px"}}>⬅️</span>
            {showSidebar && 'Logout'}
          </button>
        </div>
      </div>

      {/* Main Content */}
      <div className={`main-content ${showSidebar ? 'sidebar-expanded' : ''}`}>
        <div className="header">
          <h1 className="page-title">Store Management</h1>
        </div>

        <div className="product-form">
          <h2 className="section-title">Add New Product</h2>
          <form onSubmit={handleSubmit}>
            <div className="form-grid">
              <input
                type="text"
                name="name"
                placeholder="Product Name"
                value={newProduct.name}
                onChange={handleInputChange}
                required
                className="form-input"
              />
              <input
                type="number"
                name="price"
                placeholder="Price"
                value={newProduct.price}
                onChange={handleInputChange}
                required
                className="form-input"
              />
              <select
                name="category"
                value={newProduct.category}
                onChange={handleInputChange}
                required
                className="form-input"
              >
                <option value="">Select Category</option>
                <option value="skincare">Skincare</option>
                <option value="makeup">Makeup</option>
                <option value="haircare">Haircare</option>
                <option value="bodycare">Bodycare</option>
              </select>
              <input
                type="number"
                name="quantity"
                placeholder="Quantity"
                value={newProduct.quantity}
                onChange={handleInputChange}
                required
                className="form-input"
              />
              <textarea
                name="description"
                placeholder="Product Description"
                value={newProduct.description}
                onChange={handleInputChange}
                required
                className="form-input"
              />
            </div>
            <button type="submit" className="submit-button">Add Product</button>
          </form>
        </div>

        <div className="product-list">
          <h2 className="section-title">Product List</h2>
          <table className="product-table">
            <thead>
              <tr className="product-table-header">
                <th className="product-table-header-cell">Product Name</th>
                <th className="product-table-header-cell">Price</th>
                <th className="product-table-header-cell">Category</th>
                <th className="product-table-header-cell">Quantity</th>
                <th className="product-table-header-cell">Description</th>
                <th className="product-table-header-cell">Actions</th>
              </tr>
            </thead>
            <tbody>
              {products.map((product) => (
                <tr key={product.id} className="product-table-row">
                  <td className="product-table-cell">{product.name}</td>
                  <td className="product-table-cell">{product.price}</td>
                  <td className="product-table-cell">{product.category}</td>
                  <td className="product-table-cell">{product.quantity}</td>
                  <td className="product-table-cell">{product.description}</td>
                  <td className="product-table-cell">
                    <button 
                      onClick={() => handleDelete(product.id)}
                      className="delete-button"
                    >
                      Delete
                    </button>
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </div>
    </div>
  );
}

export default Store;