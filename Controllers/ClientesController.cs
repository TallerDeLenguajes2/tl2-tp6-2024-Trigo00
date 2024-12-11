using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using tl2_tp6_2024_Trigo00.Models;

namespace tl2_tp6_2024_Trigo00.Controllers;

public class ClientesController : Controller
{
    private readonly ILogger<ClientesController> _logger;
    private readonly IClientesRepository _clientesRepository;

    public ClientesController(ILogger<ClientesController> logger, IClientesRepository clientesRepository)
    {
        _logger = logger;
        _clientesRepository = clientesRepository;
    }

    [HttpGet]
    public IActionResult ListarClientes()
    {
        try 
        {
            var clientes = _clientesRepository.ObtenerClientes();
            return View(clientes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al listar clientes");
            return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    [HttpGet]
    public IActionResult CrearCliente()
    {
        try 
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("User"))) return RedirectToAction("Index", "Login");
            if (HttpContext.Session.GetString("Rol") != "Admin")
            {
                TempData["ErrorMessage"] = "No tienes permisos para realizar esta acción.";
                return RedirectToAction("Index");
            }
            return View();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al acceder a crear cliente");
            return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult CrearCliente(Cliente cliente)
    {
        try 
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("User"))) return RedirectToAction("Index", "Login");
            if (HttpContext.Session.GetString("Rol") != "Admin")
            {
                TempData["ErrorMessage"] = "No tienes permisos para realizar esta acción.";
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                _clientesRepository.CrearCliente(cliente);
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al crear cliente: {ClienteId}", cliente.Id);
            return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    [HttpGet]
    public IActionResult ModificarCliente(int id)
    {
        try 
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("User"))) return RedirectToAction("Index", "Login");
            if (HttpContext.Session.GetString("Rol") != "Admin")
            {
                TempData["ErrorMessage"] = "No tienes permisos para realizar esta acción.";
                return RedirectToAction("Index");
            }
            var cliente = _clientesRepository.ObtenerCliente(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al modificar cliente con ID: {ClienteId}", id);
            return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult ModificarCliente(int id, Cliente cliente)
    {
        try 
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("User"))) return RedirectToAction("Index", "Login");
            if (HttpContext.Session.GetString("Rol") != "Admin")
            {
                TempData["ErrorMessage"] = "No tienes permisos para realizar esta acción.";
                return RedirectToAction("Index");
            }

            if (ModelState.IsValid)
            {
                _clientesRepository.ModificarCliente(id, cliente);
                return RedirectToAction(nameof(Index));
            }
            return View(cliente);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al modificar cliente con ID: {ClienteId}", id);
            return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    [HttpGet]
    public IActionResult EliminarCliente(int id)
    {
        try 
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("User"))) return RedirectToAction("Index", "Login");
            if (HttpContext.Session.GetString("Rol") != "Admin")
            {
                TempData["ErrorMessage"] = "No tienes permisos para realizar esta acción.";
                return RedirectToAction("Index");
            }
            var cliente = _clientesRepository.ObtenerCliente(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al acceder a eliminar cliente con ID: {ClienteId}", id);
            return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EliminarClienteConfirmado(int id)
    {
        try 
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("User"))) return RedirectToAction("Index", "Login");
            if (HttpContext.Session.GetString("Rol") != "Admin")
            {
                TempData["ErrorMessage"] = "No tienes permisos para realizar esta acción.";
                return RedirectToAction("Index");
            }
            
            _clientesRepository.EliminarCliente(id);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al eliminar cliente con ID: {ClienteId}", id);
            return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    public IActionResult Index()
    {
        try 
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("User"))) return RedirectToAction("Index", "Login");
            ViewData["Admin"] = HttpContext.Session.GetString("Rol") == "Admin";
            return View(_clientesRepository.ObtenerClientes());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al acceder al índice de clientes");
            return View("Error", new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}