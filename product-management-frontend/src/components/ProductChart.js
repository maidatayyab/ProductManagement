import React from 'react';
import { Bar } from 'react-chartjs-2';
import { Chart as ChartJS,CategoryScale,LinearScale,BarElement,Title,Legend,} from 'chart.js';
ChartJS.register(
  CategoryScale,
  LinearScale,
  BarElement,
  Title,
  Legend
);


const ProductChart = ({ chartData }) => {
  return (
    <div style={styles.chartContainer}>
      <h3>Product Stock by Category</h3>
      {chartData ? (
        <Bar data={chartData} options={{ responsive: true }} />
      ) : (
        <p>Loading chart...</p>
      )}
    </div>
  );
};

const styles = {
  chartContainer: {
    width: '75%',
    marginBottom: '20px',
  },
};

export default ProductChart;
