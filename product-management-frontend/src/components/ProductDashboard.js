import React, { useEffect, useState } from 'react';
import { Alert, Button } from 'react-bootstrap';
import axios from 'axios';
import ProductChart from './ProductChart';
import ProductList from './ProductList';
import ProductForm from './ProductForm';

const ProductDashboard = () => {
  const [products, setProducts] = useState([]);
  const [filteredProducts, setFilteredProducts] = useState([]);
  const [chartData, setChartData] = useState(null);
  const [filter, setFilter] = useState({ name: '', productCode: '' });
  const [message, setMessage] = useState('');
  const [messageType, setMessageType] = useState('');
  const [isFormVisible, setIsFormVisible] = useState(false);

  useEffect(() => {
    fetchProductData();
  }, []);

  const fetchProductData = () => {
    axios.get('https://localhost:44348/api/product')
      .then(response => {
        setProducts(response.data);
        setFilteredProducts(response.data);
        setChartData(processChartData(response.data));
      })
      .catch(error => console.error('Error fetching products:', error));
  };

  const processChartData = (data) => {
    if (!data || data.length === 0) return null;

    const categoryStock = data.reduce((acc, product) => {
      if (!acc[product.category]) {
        acc[product.category] = 0;
      }
      acc[product.category] += product.stockQuantity;
      return acc;
    }, {});

    const categories = Object.keys(categoryStock);
    const stockQuantities = categories.map(category => categoryStock[category]);

    return {
      labels: categories,
      datasets: [{
        label: 'Product Stock by Category',
        data: stockQuantities,
        backgroundColor: ['#FF5733', '#33FF57', '#3357FF'],
      }],
    };
  };
  
  const handleSaveProduct = (updatedProduct) => {
  const updatedProducts = products.map((product) =>
    product.id === updatedProduct.id ? { ...product, ...updatedProduct } : product
  );

  setProducts(updatedProducts);
  setFilteredProducts(updatedProducts);  
  setMessage('Product updated successfully!');
  setMessageType('success');
  setTimeout(() => setMessage(''), 3000);
};

  const handleFilterChange = (e) => {
    const { name, value } = e.target;
    const updatedFilter = { ...filter, [name]: value };
    setFilter(updatedFilter);
    applyFilter(updatedFilter);
  };

  const applyFilter = (filterCriteria) => {
    let filtered = products;

    if (filterCriteria.name) {
      filtered = filtered.filter((product) =>
        product.name.toLowerCase().includes(filterCriteria.name.toLowerCase())
      );
    }

    if (filterCriteria.productCode) {
      filtered = filtered.filter((product) =>
        product.productCode.toLowerCase().includes(filterCriteria.productCode.toLowerCase())
      );
    }

    setFilteredProducts(filtered);
  };

  const handleDeleteProduct = (productId) => {
    axios.delete(`https://localhost:44348/api/product/${productId}`)
      .then(() => {
        setMessage('Product deleted successfully!');
        setMessageType('success');
        setTimeout(() => setMessage(''), 3000);
        fetchProductData();
      })
      .catch(() => {
        setMessage('Failed to delete product. Please try again.');
        setMessageType('danger');
        setTimeout(() => setMessage(''), 3000);
      });
  };

  const addNewProduct = (newProduct) => {
    setProducts((prevProducts) => {
      const updatedProducts = [...prevProducts, newProduct];
      setFilteredProducts(updatedProducts);
      setChartData(processChartData(updatedProducts)); // Update chart data
      return updatedProducts;
    });

    setMessage('Product created successfully!');
    setMessageType('success');
    setTimeout(() => setMessage(''), 3000);
    setIsFormVisible(false); 
  };

  return (
    <div style={styles.container}>
      {/* Chart Component */}
      <ProductChart chartData={chartData} />

      {/* Create Product Form */}
      {isFormVisible && <ProductForm addNewProduct={addNewProduct} />}

      {/* Button to toggle form visibility */}
      <Button variant="secondary" onClick={() => setIsFormVisible(!isFormVisible)} style={styles.CreateButton}>
        {isFormVisible ? 'Cancel' : 'Create Product'}
      </Button>
	  
		{/* Success/Failure Message */}
      {message && <Alert variant={messageType}>{message}</Alert>}
	  
      {/* Filter Section */}
      <div style={styles.filterContainer}>
        <input
          type="text"
          placeholder="Search by Name"
          name="name"
          value={filter.name}
          onChange={handleFilterChange}
        />
        <input
          type="text"
          placeholder="Search by Product Code"
          name="productCode"
          value={filter.productCode}
          onChange={handleFilterChange}
        />
      </div>

      {/* Product List */}
       <ProductList
      filteredProducts={filteredProducts}
      handleDeleteProduct={handleDeleteProduct}
      handleSaveProduct={handleSaveProduct} 
    />
    </div>
  );
};

const styles = {
  container: {
    display: 'flex',
    flexDirection: 'column',
    alignItems: 'center',
    justifyContent: 'center',
    width: '100%',
    padding: '20px',
  },
  filterContainer: {
    marginBottom: '20px',
	width: '75%',
  },
  CreateButton: {
    margin: '20px',
    backgroundColor: '#0d6efd',
  },
};

export default ProductDashboard;
