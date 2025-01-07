import React, { useState } from 'react';  
import { Table, Button, Form } from 'react-bootstrap';  
import axios from 'axios';

const ProductList = ({ filteredProducts, handleDeleteProduct, handleSaveProduct }) => {
  const [sortConfig, setSortConfig] = useState({
    key: 'name',
    direction: 'ascending',
  });
  const [editingProductId, setEditingProductId] = useState(null);
  const [editedProduct, setEditedProduct] = useState({});
  const sortData = (data, sortKey, direction) => {
    const sortedData = [...data].sort((a, b) => {
      if (a[sortKey] < b[sortKey]) {
        return direction === 'ascending' ? -1 : 1;
      }
      if (a[sortKey] > b[sortKey]) {
        return direction === 'ascending' ? 1 : -1;
      }
      return 0;
    });
    return sortedData;
  };

  const handleSort = (key) => {
    let direction = 'ascending';
    if (sortConfig.key === key && sortConfig.direction === 'ascending') {
      direction = 'descending';
    }
    setSortConfig({ key, direction });
  };

  const handleEdit = (product) => {
    setEditingProductId(product.id);
    setEditedProduct({ ...product });  
  };

  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setEditedProduct((prev) => ({
      ...prev,
      [name]: value,
    }));
  };

  const handleSave = async () => {
  try {
    const updatedProduct = { 
	  id: editedProduct.id,
      name: editedProduct.name, 
      price: editedProduct.price,
    };

    await axios.put(`https://localhost:44348/api/product/${editedProduct.id}`, updatedProduct);

	console.log(updatedProduct);
    handleSaveProduct(updatedProduct);  

    setEditingProductId(null);
  } catch (error) {
    console.error("Error updating product:", error);
    alert("Error updating product. Please try again.");
  }
};

  const handleCancel = () => {
    setEditingProductId(null);  
  };

  const sortedProducts = sortData(filteredProducts, sortConfig.key, sortConfig.direction);

  return (
    <div style={styles.tableContainer}>
      <h3>Product List</h3>
      <Table striped bordered hover responsive="md" style={styles.table}>
        <thead>
          <tr>
            <th onClick={() => handleSort('name')}>Name {sortConfig.key === 'name' && (sortConfig.direction === 'ascending' ? '↑' : '↓')}</th>
            <th onClick={() => handleSort('price')}>Price {sortConfig.key === 'price' && (sortConfig.direction === 'ascending' ? '↑' : '↓')}</th>
            <th onClick={() => handleSort('category')}>Category {sortConfig.key === 'category' && (sortConfig.direction === 'ascending' ? '↑' : '↓')}</th>
            <th onClick={() => handleSort('stockQuantity')}>Stock Quantity {sortConfig.key === 'stockQuantity' && (sortConfig.direction === 'ascending' ? '↑' : '↓')}</th>
            <th onClick={() => handleSort('productCode')}>Product Code {sortConfig.key === 'productCode' && (sortConfig.direction === 'ascending' ? '↑' : '↓')}</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {sortedProducts.map((product) => (
            <tr key={product.id}>
              {editingProductId === product.id ? (
                <>
                  <td>
                    <Form.Control
                      type="text"
                      name="name"
                      value={editedProduct.name}
                      onChange={handleInputChange}
                    />
                  </td>
                  <td>
                    <Form.Control
                      type="number"
                      name="price"
                      value={editedProduct.price}
                      onChange={handleInputChange}
                    />
                  </td>
                  {/* Non-editable fields */}
                  <td>{product.category}</td>
                  <td>{product.stockQuantity}</td>
                  <td>{product.productCode}</td>
                  <td>
                    <Button variant="success" onClick={handleSave}>Save</Button>
                    <Button variant="secondary" onClick={handleCancel}>Cancel</Button>
                  </td>
                </>
              ) : (
                // Non-editable row
                <>
                  <td>{product.name}</td>
                  <td>{product.price}</td>
                  <td>{product.category}</td>
                  <td>{product.stockQuantity}</td>
                  <td>{product.productCode}</td>
                  <td>
                    <Button variant="warning" onClick={() => handleEdit(product)}>
                      Edit
                    </Button>
                    <Button variant="danger" onClick={() => handleDeleteProduct(product.id)}>
                      Delete
                    </Button>
                  </td>
                </>
              )}
            </tr>
          ))}
        </tbody>
      </Table>
    </div>
  );
};

const styles = {
  tableContainer: {
    width: '75%',
    marginBottom: '20px',
  },
  table: {
    borderCollapse: 'collapse',
    width: '100%',
  },
};

export default ProductList; 
