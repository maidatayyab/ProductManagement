import React, { useState } from 'react';
import { Form, Row, Col, Button } from 'react-bootstrap';
import axios from 'axios';

const ProductForm = ({ addNewProduct }) => {
  const [newProduct, setNewProduct] = useState({
    name: '',
    category: '',
    productCode: '',
    price: '',
    stockQuantity: '',
  });
  const [message, setMessage] = useState('');
  const [messageType, setMessageType] = useState('');

  const handleProductChange = (e) => {
    const { name, value } = e.target;
    setNewProduct((prevProduct) => ({ ...prevProduct, [name]: value }));
  };

  const handleCreateProduct = () => {
    axios.post('https://localhost:44348/api/product', newProduct)
      .then(response => {
        addNewProduct(response.data); 
        setNewProduct({
          name: '',
          category: '',
          productCode: '',
          price: '',
          stockQuantity: '',
        });
      })
      .catch(error => {
        setMessage('Failed to create product. Please try again.');
        setMessageType('danger');
        setTimeout(() => setMessage(''), 3000);
      });
  };

  return (
    <div style={styles.formContainer}>
      <h4>Create Product</h4>
      <Form>
        <Row className="mb-3">
          <Col sm={3}>
            <Form.Label>Name</Form.Label>
          </Col>
          <Col>
            <Form.Control
              type="text"
              name="name"
              value={newProduct.name}
              onChange={handleProductChange}
              placeholder="Enter product name"
            />
          </Col>
        </Row>

        <Row className="mb-3">
          <Col sm={3}>
            <Form.Label>Category</Form.Label>
          </Col>
          <Col>
            <Form.Control
              type="text"
              name="category"
              value={newProduct.category}
              onChange={handleProductChange}
              placeholder="Enter product category"
            />
          </Col>
        </Row>

        <Row className="mb-3">
          <Col sm={3}>
            <Form.Label>Product Code</Form.Label>
          </Col>
          <Col>
            <Form.Control
              type="text"
              name="productCode"
              value={newProduct.productCode}
              onChange={handleProductChange}
              placeholder="Enter product code"
            />
          </Col>
        </Row>

        <Row className="mb-3">
          <Col sm={3}>
            <Form.Label>Price</Form.Label>
          </Col>
          <Col>
            <Form.Control
              type="number"
              name="price"
              value={newProduct.price}
              onChange={handleProductChange}
              placeholder="Enter product price"
            />
          </Col>
        </Row>

        <Row className="mb-3">
          <Col sm={3}>
            <Form.Label>Stock Quantity</Form.Label>
          </Col>
          <Col>
            <Form.Control
              type="number"
              name="stockQuantity"
              value={newProduct.stockQuantity}
              onChange={handleProductChange}
              placeholder="Enter stock quantity"
            />
          </Col>
        </Row>
        <Button variant="primary" onClick={handleCreateProduct} style={{ marginTop: '10px' }}>
          Save Product
        </Button>
      </Form>
    </div>
  );
};

const styles = {
  formContainer: {
    marginTop: '20px',
    display: 'flex',
    flexDirection: 'column',
    width: '75%',
    padding: '20px',
    border: '1px solid #ccc',
    borderRadius: '5px',
  },
};

export default ProductForm;
