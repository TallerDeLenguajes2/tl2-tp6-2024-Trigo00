using Microsoft.AspNetCore.Mvc;

public class LoginController : Controller
{
    private readonly ILogger<LoginController> _logger;
    private readonly IUserRepository _userRepository;

    public LoginController(ILogger<LoginController> logger, IUserRepository userRepository)
    {
        _logger = logger;
        _userRepository = userRepository;
    }

    public IActionResult Login(LoginViewModel model)
    {
        try
        {
            // Verificar que los datos de entrada no estén vacíos
            if (string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
            {
                model.ErrorMessage = "Por favor ingrese su nombre de usuario y contraseña.";
                return View("Index", model);
            }

            // Intentar obtener el usuario
            Usuario user = _userRepository.ObtenerUsuario(model.Username, model.Password);
            if (user != null)
            {
                // Redirigir a la página principal o dashboard
                HttpContext.Session.SetString("IsAuthenticated", "True");
                HttpContext.Session.SetString("User", user.Username);
                HttpContext.Session.SetString("Rol", user.Rol.ToString());

                _logger.LogInformation("El usuario {Username} ingresó correctamente", model.Username);
                return RedirectToAction("Index", "Productos");
            }

            // Si las credenciales no son correctas, mostrar mensaje de error
            _logger.LogWarning("Intento de acceso inválido - Usuario: {Username}", model.Username);
            ViewBag.ErrorMessage = "Credenciales Inválidas.";
            model.IsAuthenticated = false;
            return View("Index", model);
        }
        catch (Exception ex)
        {
            // Loguear el error y retornar una vista de error
            _logger.LogError(ex, "Ocurrió un error inesperado durante el proceso de login para el usuario {Username}", model.Username);
            ViewBag.ErrorMessage = "Ocurrió un error inesperado. Por favor, intente nuevamente más tarde.";
            return View("Index", model);
        }
    }

    public IActionResult Index()
    {
        try
        {
            var model = new LoginViewModel
            {
                IsAuthenticated = HttpContext.Session.GetString("IsAuthenticated") == "True",
            };
            return View(model);
        }
        catch (Exception ex)
        {
            // Loguear el error y mostrar una vista de error genérica
            _logger.LogError(ex, "Ocurrió un error al cargar la página de inicio de sesión.");
            ViewBag.ErrorMessage = "Ocurrió un error inesperado. Por favor, intente nuevamente más tarde.";
            return View("Error");
        }
    }

    public IActionResult Logout()
    {
        try
        {
            // Limpiar la sesión
            HttpContext.Session.Clear();

            // Redirigir a la vista de login
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            // Loguear el error y mostrar una vista de error genérica
            _logger.LogError(ex, "Ocurrió un error durante el proceso de cierre de sesión.");
            ViewBag.ErrorMessage = "Ocurrió un error inesperado. Por favor, intente nuevamente más tarde.";
            return View("Error");
        }
    }
}
