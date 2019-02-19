using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CustomerSupplierCodingChallenge.Models;

namespace CustomerSupplierCodingChallenge
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Random random = new Random();
        List<Customer> customerList;
        List<Supplier> supplierList;

        SolidColorBrush mySolidColorBrush = new SolidColorBrush();
        

        public MainWindow()
        {
            InitializeComponent();
        }

        private void initializeCanvas()
        {
            graphCanvas.Children.Clear();
            this.customerList = new List<Customer>();
            this.supplierList = new List<Supplier>();
            errorBlock.Text = "";
        }

        private void onDrawBtnClicked(object sender, RoutedEventArgs e)
        {
            initializeCanvas();

            int noCustomers, noSuppliers, gridX, gridY;

            //get user input
            try
            {
                noCustomers = Convert.ToInt32(customerInput.Text);
                noSuppliers = Convert.ToInt32(supplierInput.Text);
                gridX = Convert.ToInt32(gridXInput.Text);
                gridY = Convert.ToInt32(gridYInput.Text);

                drawGrid(gridX, gridY);
                plotCustomers(noCustomers);
                plotSuppliers(noSuppliers);
                assignSuppliers();
            }
            catch (Exception ex)
            {
                errorBlock.Text = "There was a problem converting the user input.";
            }
        }

        /**
         * Draws the lines and the scale for the grid that the customer and
         * supplier points will be plotted on
         **/
        private void drawGrid(int gridX, int gridY)
        {
            //get the number of x and y (vertical and horizontal) lines based on user input
            int noGridX = ((gridX % 50) == 0) ? gridX / 50 : gridX / 50 + 1;
            int noGridY = ((gridY % 50) == 0) ? gridY / 50 : gridY / 50 + 1;

            drawXAxis(gridX, noGridX, noGridY);
            drawYAxis(gridY, noGridX, noGridY);
        }

        /**
         * Draws the vertical grid lines on the x-axis
         **/
        private void drawXAxis(int gridX, int noGridX, int noGridY)
        {
            for (int i = -1; i < noGridX; i++)
            {
                var x = graphCanvas.Width / noGridX * (i + 1);
                var x2 = gridX / noGridX * (i + 1);

                //Add new line
                System.Windows.Shapes.Line line = new System.Windows.Shapes.Line();
                line.Stroke = System.Windows.Media.Brushes.Black;
                line.X1 = x;
                line.Y1 = 0;
                line.X2 = x;
                line.Y2 = graphCanvas.Height;
                line.StrokeThickness = 1;

                graphCanvas.Children.Add(line);

                //draw the scale numbers
                drawXScale(x2, x);
            }
        }

        /**
         * Draws the horizontal grid lines on the y-axis
         **/
        private void drawYAxis(int gridY, int noGridX, int noGridY)
        {
            for (int i = -1; i < noGridX; i++)
            {
                var y = graphCanvas.Height / noGridX * (i + 1);
                var y2 = gridY / noGridY * (i + 1);

                //Add new line
                System.Windows.Shapes.Line line = new System.Windows.Shapes.Line();
                line.Stroke = System.Windows.Media.Brushes.Black;
                line.X1 = 0;
                line.Y1 = y;
                line.X2 = graphCanvas.Width;
                line.Y2 = y;
                line.StrokeThickness = 1;

                graphCanvas.Children.Add(line);

                //draw the scale numbers
                drawYScale(y2, y, gridY);
            }
        }

        /**
         * Draws the scale (numbers) on the x-axis
         **/
        private void drawXScale(double factor, double x)
        {
            if(factor != 0)
            {
                System.Windows.Controls.TextBlock label = new System.Windows.Controls.TextBlock();
                Grid.SetRow(label, 1);
                Grid.SetColumn(label, 1);
                Grid.SetRowSpan(label, 2);
                Grid.SetColumnSpan(label, 3);

                label.Text = factor.ToString();
                label.Padding = new Thickness(2);
                label.Margin = new Thickness(x - (x/ 20), graphCanvas.Height + 6, 0, 0);
                graphCanvas.Children.Add(label);
            }
        }

        /**
         * Draws the scale (numbers) on the y-axis
         **/
        private void drawYScale(double factor, double y, int gridY)
        {
            System.Windows.Controls.TextBlock label = new System.Windows.Controls.TextBlock();
            Grid.SetRow(label, 1);
            Grid.SetColumn(label, 1);
            Grid.SetRowSpan(label, 3);
            Grid.SetColumnSpan(label, 2);

            label.Text = (gridY - factor).ToString();
            label.Margin = new Thickness(-25, y - (y/20), 0, 0);
            graphCanvas.Children.Add(label);
        }

        /**
         * Creates the customer object and adds its location to the graph
         **/
        private void plotCustomers(int noCustomers)
        {
            for (int i = 0; i < noCustomers; i++)
            {
                //get coordinates
                double[] coord = generateRandomCoordinates();

                //create customer object
                this.customerList.Add(createCustomer(coord));

                //add the point to the graph
                System.Windows.Shapes.Rectangle customerPoint = new System.Windows.Shapes.Rectangle();
                Grid.SetRow(customerPoint, 1);
                Grid.SetColumn(customerPoint, 1);
                Grid.SetColumnSpan(customerPoint, 1);

                customerPoint.Stroke = System.Windows.Media.Brushes.Blue;
                customerPoint.Width = 4;
                customerPoint.Height = 4;
                customerPoint.HorizontalAlignment = HorizontalAlignment.Left;
                customerPoint.VerticalAlignment = VerticalAlignment.Top;
                customerPoint.Margin = new Thickness(coord[0], coord[1], 0, 0);
                customerPoint.StrokeThickness = 3;

                graphCanvas.Children.Add(customerPoint);
                
            }
        }

        /**
         * Creates the supplier object and adds its location to the graph
         **/
        private void plotSuppliers(int noSuppliers)
        {
            for (int i = 0; i < noSuppliers; i++)
            {
                //get the coordinates
                double[] coord = generateRandomCoordinates();

                //create the supplier object
                this.supplierList.Add(createSupplier(coord, i));

                //add the point to the graph
                System.Windows.Shapes.Rectangle supplierPoint = new System.Windows.Shapes.Rectangle();
                Grid.SetRow(supplierPoint, 1);
                Grid.SetColumn(supplierPoint, 1);
                Grid.SetColumnSpan(supplierPoint, 1);

                supplierPoint.Stroke = System.Windows.Media.Brushes.Red;
                supplierPoint.Width = 4;
                supplierPoint.Height = 4;
                supplierPoint.HorizontalAlignment = HorizontalAlignment.Left;
                supplierPoint.VerticalAlignment = VerticalAlignment.Top;
                supplierPoint.Margin = new Thickness(coord[0], coord[1], 0, 0);
                supplierPoint.StrokeThickness = 3;

                graphCanvas.Children.Add(supplierPoint);
            }
        }

        /**
         * Method called to generate two random x and y coordinates
         **/
        private double[] generateRandomCoordinates()
        {
            double[] coordinates = new double[2];

            var next = random.NextDouble();
            coordinates[0] = random.NextDouble() * (graphCanvas.Width - 0) + 0;
            coordinates[1] = random.NextDouble() * (graphCanvas.Height - 0) + 0;

            return coordinates;
        }

        /**
         * Creates a customer object with x,y coordinates
         **/
        private Customer createCustomer(double[] coord)
        {
            return new Customer(coord[0], coord[1]);
        }

        /**
         * Creates supplier object with x,y coordinates and an ID (for colour coding purposes)
         **/
        private Supplier createSupplier(double[] coord, int id)
        {
            Supplier supplier = new Supplier(coord[0], coord[1], id);

            setBrushColour(supplier);

            return supplier;
        }

        /**
         * Determines which supplier is closer to each customer and assigns that
         * supplier to the customer through the customer model. Draws a line on the
         * graph from the customer to their respective supplier.
         **/
        private void assignSuppliers()
        {
            foreach (Customer customer in customerList) {
                List<KeyValuePair<Supplier, float>> distanceList = new List<KeyValuePair<Supplier, float>>();
                distanceList = getDistanceToEachSupplier(customer, distanceList);

                //sort list
                distanceList.Sort((x, y) => x.Value.CompareTo(y.Value));

                //assign closest supplier to customer
                customer.supplier = distanceList[0].Key;
                
                //draw line
                addCustomerSupplierConnection(customer);
            }
        }

        /**
         * Determines which supplier is closer to the customer provided
         **/
        private List<KeyValuePair<Supplier, float>> getDistanceToEachSupplier(Customer customer, List<KeyValuePair<Supplier, float>> distanceList)
        {
            foreach(Supplier supplier in this.supplierList)
            {
                //Compute distance to supplier and add to list
                float distance = (float)Math.Sqrt(Math.Pow(supplier.x - customer.x, 2) + Math.Pow(supplier.y - customer.y, 2));
                distanceList.Add(new KeyValuePair<Supplier, float>(supplier, distance));
            }

            return distanceList;
        }

        /**
         * Draws the line between the customer and its matched supplier
         **/
        private void addCustomerSupplierConnection(Customer customer)
        {
            System.Windows.Shapes.Line line = new System.Windows.Shapes.Line();
            line.Stroke = customer.supplier.colour;
            line.HorizontalAlignment = HorizontalAlignment.Left;
            line.VerticalAlignment = VerticalAlignment.Top;
            line.X1 = customer.x;
            line.Y1 = customer.y;
            line.X2 = customer.supplier.x;
            line.Y2 = customer.supplier.y;
            line.StrokeThickness = 2;

            graphCanvas.Children.Add(line);
        }

        /**
         * Initializes a list of colours gathered from System.Windows.Media.Brushes
         * and assigns the colour corresponding to the list of suppliers
         **/
        private void setBrushColour(Supplier supplier)
        {
            //initialize colours
            List<System.Windows.Media.SolidColorBrush> brushColours = new List<System.Windows.Media.SolidColorBrush>();
            brushColours.Add(System.Windows.Media.Brushes.Brown);
            brushColours.Add(System.Windows.Media.Brushes.Yellow);
            brushColours.Add(System.Windows.Media.Brushes.Green);
            brushColours.Add(System.Windows.Media.Brushes.Gray);
            brushColours.Add(System.Windows.Media.Brushes.DarkSeaGreen);
            brushColours.Add(System.Windows.Media.Brushes.GreenYellow);
            brushColours.Add(System.Windows.Media.Brushes.Beige);
            brushColours.Add(System.Windows.Media.Brushes.MediumPurple);
            brushColours.Add(System.Windows.Media.Brushes.Pink);
            brushColours.Add(System.Windows.Media.Brushes.SandyBrown);
            brushColours.Add(System.Windows.Media.Brushes.Orange);
            brushColours.Add(System.Windows.Media.Brushes.CornflowerBlue);

            //set colour property of Supplier
            supplier.colour = brushColours[supplier.id];
        }
    }
}
