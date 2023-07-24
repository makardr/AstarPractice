using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.Windowing;

public class SilkWindow
{
    private static IWindow _window;

    public SilkWindow()
    {
        
        Initialize();
    }

    public void Initialize()
    {
        WindowOptions options = WindowOptions.Default;
        options.Size = new Vector2D<int>(800, 600);
        options.Title = "Silk.NET program";
        _window = Window.Create(options);
        _window.Load += OnLoad;
        _window.Update += OnUpdate;
        _window.Render += OnRender;
        _window.Run();
    }

    private static void OnLoad()
    {
        // Console.WriteLine("Load!");
        IInputContext input = _window.CreateInput();
        for (int i = 0; i < input.Keyboards.Count; i++)
            input.Keyboards[i].KeyDown += KeyDown;
    }

    private static void OnUpdate(double deltaTime)
    {
        // Console.WriteLine("Update!");
    }

    private static void OnRender(double deltaTime)
    {
        // Console.WriteLine("Render!");
    }

    private static void KeyDown(IKeyboard keyboard, Key key, int keyCode)
    {
        if (key == Key.Escape)
            _window.Close();
    }
}