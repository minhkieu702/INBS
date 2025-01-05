import React, { useState } from 'react';
import '../css/Store.css';

function Store() {
  const [products, setProducts] = useState([]);
  const [newProduct, setNewProduct] = useState({
    name: '',
    price: '',
    category: '',
    quantity: '',
    description: ''
  });

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

  return (
    <div className="store-container">
      <h1>Quản lý Store</h1>

      <div className="product-form">
        <h2>Thêm sản phẩm mới</h2>
        <form onSubmit={handleSubmit}>
          <div className="form-group">
            <input
              type="text"
              name="name"
              placeholder="Tên sản phẩm"
              value={newProduct.name}
              onChange={handleInputChange}
              required
            />
          </div>
          <div className="form-group">
            <input
              type="number"
              name="price"
              placeholder="Giá"
              value={newProduct.price}
              onChange={handleInputChange}
              required
            />
          </div>
          <div className="form-group">
            <select
              name="category"
              value={newProduct.category}
              onChange={handleInputChange}
              required
            >
              <option value="">Chọn danh mục</option>
              <option value="skincare">Skincare</option>
              <option value="makeup">Makeup</option>
              <option value="haircare">Haircare</option>
              <option value="bodycare">Bodycare</option>
            </select>
          </div>
          <div className="form-group">
            <input
              type="number"
              name="quantity"
              placeholder="Số lượng"
              value={newProduct.quantity}
              onChange={handleInputChange}
              required
            />
          </div>
          <div className="form-group">
            <textarea
              name="description"
              placeholder="Mô tả sản phẩm"
              value={newProduct.description}
              onChange={handleInputChange}
              required
            />
          </div>
          <button type="submit">Thêm sản phẩm</button>
        </form>
      </div>

      <div className="product-list">
        <h2>Danh sách sản phẩm</h2>
        <table>
          <thead>
            <tr>
              <th>Tên sản phẩm</th>
              <th>Giá</th>
              <th>Danh mục</th>
              <th>Số lượng</th>
              <th>Mô tả</th>
              <th>Thao tác</th>
            </tr>
          </thead>
          <tbody>
            {products.map((product) => (
              <tr key={product.id}>
                <td>{product.name}</td>
                <td>{product.price}đ</td>
                <td>{product.category}</td>
                <td>{product.quantity}</td>
                <td>{product.description}</td>
                <td>
                  <button 
                    className="delete-btn"
                    onClick={() => handleDelete(product.id)}
                  >
                    Xóa
                  </button>
                </td>
              </tr>
            ))}
          </tbody>
        </table>
      </div>
    </div>
  );
}

export default Store;