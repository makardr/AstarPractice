using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.Windowing;
using Silk.NET.OpenGL;

public class SilkWindow
{
    private static IWindow _window;
    private static GL gl;

    public SilkWindow()
    {
        Initialize();
    }

    public void Initialize()
    {
        WindowOptions options = WindowOptions.Default;
        options.Size = new Vector2D<int>(2400, 1600);
        options.Title = "Silk.NET program";
        _window = Window.Create(options);
        _window.Load += OnLoad;
        _window.Update += OnUpdate;
        _window.Render += OnRender;
        _window.Run();
    }

    private static void OnLoad()
    {
        IInputContext input = _window.CreateInput();
        gl = _window.CreateOpenGL();
        gl.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);

        // Console.WriteLine("Load!");
        
        //Input
        for (int i = 0; i < input.Keyboards.Count; i++)
            input.Keyboards[i].KeyDown += KeyDown;
    }

    private static void OnUpdate(double deltaTime)
    {
        // Console.WriteLine("Update!");
    }

    private static void OnRender(double deltaTime)
    {
        gl.Clear(ClearBufferMask.ColorBufferBit);
        
        
    }

    private static void KeyDown(IKeyboard keyboard, Key key, int keyCode)
    {
        if (key == Key.Escape)
            _window.Close();
    }
}