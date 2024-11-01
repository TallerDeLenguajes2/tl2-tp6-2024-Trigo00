using Microsoft.Data.Sqlite;
using ModelsProducto;

namespace productoRepository;

class ProductoRepository
{
    private string connectionString = @"Data Source = db/Tienda.db;Cache=Shared";

    public void CrearProducto(Producto producto)
    {
        string query = @"INSERT INTO Productos (Descripcion, Precio) VALUES (@Descripcion, @Precio)";

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query,connection);
            command.Parameters.AddWithValue("@Descripcion", producto.Descripcion);
            command.Parameters.AddWithValue("@Precio", producto.Precio);
            command.ExecuteNonQuery();
            connection.Close();            
        }

    }

    public List<Producto> ObtenerProductos()
    {
        List<Producto> productos = new List<Producto>();

        string query = @"SELECT * FROM Productos";

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query,connection);

            using (SqliteDataReader reader = command.ExecuteReader())
            {
                while(reader.Read())
                {
                    Producto nuevoProducto = new Producto();
                    nuevoProducto.IdProducto = Convert.ToInt32(reader["idProducto"]);
                    nuevoProducto.Descripcion = reader["Descripcion"].ToString();
                    nuevoProducto.Precio = Convert.ToInt32(reader["Precio"]);
                    productos.Add(nuevoProducto);
                }
            }
            connection.Close();            
        }
        return productos;
    }

    public void ModificarProducto(int id, Producto producto)
    {
        string query = @"UPDATE Productos SET Descripcion = @Descripcion, Precio = @Precio WHERE idProducto = @Id";

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query,connection);
            command.Parameters.AddWithValue("@Descripcion", producto.Descripcion);
            command.Parameters.AddWithValue("@Precio", producto.Precio);
            command.Parameters.AddWithValue("@Id", id);
            command.ExecuteNonQuery();
            connection.Close();            
        }

    }

    public Producto ObtenerProducto(int id)
    {
        Producto producto = null; //Uso el null para devolver en caso de no encontrar nada

        string query = @"SELECT * FROM Productos WHERE idProducto = @id ";

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query,connection);
            command.Parameters.AddWithValue("@id", id);
            using (SqliteDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    producto = new Producto();
                    producto.IdProducto = Convert.ToInt32(reader["idProducto"]);
                    producto.Descripcion = reader["Descripcion"].ToString();
                    producto.Precio = Convert.ToInt32(reader["Precio"]);
                }
            }
            connection.Close();            
        }
        return producto;
    }

    public void EliminarProducto(int id)
    {
        string query = @"DELETE FROM Productos WHERE idProducto = @Id;";

        using (SqliteConnection connection = new SqliteConnection(connectionString))
        {
            connection.Open();
            SqliteCommand command = new SqliteCommand(query,connection);
            command.Parameters.AddWithValue("@id", id);
            command.ExecuteNonQuery();
            connection.Close();            
        }
    }
}