using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.Windowing;
using Silk.NET.OpenGL;


public class SilkWindow
{
    private static IWindow _window;
    private static GL _gl;
    private static uint _program;
    private static uint _vao;
    private static uint _vbo;
    private static uint _ebo; 
    private static float rotation = 0.0f;
    private static float[] vertices =
    {
        0.5f,  0.5f, 0.0f,
        0.5f, -0.5f, 0.0f,
        -0.5f, -0.5f, 0.0f,
        -0.5f,  0.5f, 0.0f
    };
    public SilkWindow()
    {
        WindowOptions options = WindowOptions.Default;
        options.Size = new Vector2D<int>(1270, 720);
        options.Title = "Silk.NET program";
        _window = Window.Create(options);
        
        _window.Load += OnLoad;
        _window.Update += OnUpdate;
        _window.Render += OnRender;
        
        _window.Run();
    }

    private static unsafe void OnLoad()
    {
        IInputContext input = _window.CreateInput();
        _gl = GL.GetApi(_window);
        
        _gl.ClearColor(1.0f, 1.0f, 1.0f, 1.0f);

        _vao = _gl.GenVertexArray();
        _gl.BindVertexArray(_vao);
        
        _vbo = _gl.GenBuffer();
        _gl.BindBuffer(BufferTargetARB.ArrayBuffer, _vbo);
        
        _ebo = _gl.GenBuffer();
        _gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, _ebo);
        
        float[] vertices =
        {
            0.5f,  0.5f, 0.0f,
            0.5f, -0.5f, 0.0f,
            -0.5f, -0.5f, 0.0f,
            -0.5f,  0.5f, 0.0f
        };
        
        uint[] indices =
        {
            0u, 1u, 3u,
            1u, 2u, 3u
        };
        
        
        // Initialize buffers the first time 
        fixed (float* buf = vertices) _gl.BufferData(BufferTargetARB.ArrayBuffer, (nuint) (vertices.Length * sizeof(float)), buf, BufferUsageARB.StaticDraw);
        fixed (uint* buf = indices) _gl.BufferData(BufferTargetARB.ElementArrayBuffer, (nuint) (indices.Length * sizeof(uint)), buf, BufferUsageARB.StaticDraw);
        
        
        const string vertexCode = @"
            #version 330 core

            layout (location = 0) in vec3 aPosition;

            void main()
            {
                gl_Position = vec4(aPosition, 1.0);
            }";
        
        const string fragmentCode = @"
            #version 330 core

            out vec4 out_color;

            void main()
            {
                out_color = vec4(1.0, 0.5, 0.2, 1.0);
            }";
        
        uint vertexShader = _gl.CreateShader(ShaderType.VertexShader);
        _gl.ShaderSource(vertexShader, vertexCode);
        
        _gl.CompileShader(vertexShader);
        
        // Debug shader log
        _gl.GetShader(vertexShader, ShaderParameterName.CompileStatus, out int vStatus);
        if (vStatus != (int) GLEnum.True)
            throw new Exception("Vertex shader failed to compile: " + _gl.GetShaderInfoLog(vertexShader));
        
        
        uint fragmentShader = _gl.CreateShader(ShaderType.FragmentShader);
        _gl.ShaderSource(fragmentShader, fragmentCode);

        _gl.CompileShader(fragmentShader);

        _gl.GetShader(fragmentShader, ShaderParameterName.CompileStatus, out int fStatus);
        if (fStatus != (int) GLEnum.True)
            throw new Exception("Fragment shader failed to compile: " + _gl.GetShaderInfoLog(fragmentShader));

        _program = _gl.CreateProgram();
        
        
        _gl.AttachShader(_program, vertexShader);
        _gl.AttachShader(_program, fragmentShader);

        _gl.LinkProgram(_program);

        _gl.GetProgram(_program, ProgramPropertyARB.LinkStatus, out int lStatus);
        if (lStatus != (int) GLEnum.True)
            throw new Exception("Program failed to link: " + _gl.GetProgramInfoLog(_program));
        
        _gl.DetachShader(_program, vertexShader);
        _gl.DetachShader(_program, fragmentShader);
        _gl.DeleteShader(vertexShader);
        _gl.DeleteShader(fragmentShader);
        
        
        const uint positionLoc = 0;
        _gl.EnableVertexAttribArray(positionLoc);
        _gl.VertexAttribPointer(positionLoc, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), (void*) 0);
        
        _gl.BindVertexArray(0);
        _gl.BindBuffer(BufferTargetARB.ArrayBuffer, 0);
        _gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, 0);
        
        
        _gl.BindBuffer(BufferTargetARB.ArrayBuffer, 0); // Unbind buffer
        _gl.EnableVertexAttribArray(0);
        
        // Console.WriteLine("Load!");
        //Input
        for (int i = 0; i < input.Keyboards.Count; i++)
            input.Keyboards[i].KeyDown += KeyDown;
    }

    private static void OnUpdate(double deltaTime)
    {
        rotation += 0.01f;
        
        float sinRotation = (float)Math.Sin(rotation);
        float[] verticies = {
            -0.5f + sinRotation, -0.5f, 0.0f,
            0.5f + sinRotation, -0.5f, 0.0f,
            0.0f + sinRotation, 0.5f, 0.0f,
            -0.5f + sinRotation,  0.5f, 0.0f
        };
        Console.WriteLine(verticies[0]);
    }

    private static unsafe void OnRender(double deltaTime)
    {
        _vao = _gl.GenVertexArray();
        _gl.BindVertexArray(_vao);
        
        _vbo = _gl.GenBuffer();
        _gl.BindBuffer(BufferTargetARB.ArrayBuffer, _vbo);
        
        _ebo = _gl.GenBuffer();
        _gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, _ebo);
        
        
        _gl.Clear(ClearBufferMask.ColorBufferBit);
        
        _gl.BindVertexArray(_vao);
        _gl.UseProgram(_program);
        
        _gl.DrawElements(PrimitiveType.Triangles, 6, DrawElementsType.UnsignedInt, (void*) 0);
        
        _gl.BindVertexArray(0);
        _gl.BindBuffer(BufferTargetARB.ElementArrayBuffer, 0);
        _gl.BindBuffer(BufferTargetARB.ArrayBuffer, 0);
    }

    private static void KeyDown(IKeyboard keyboard, Key key, int keyCode)
    {
        if (key == Key.Escape)
            _window.Close();
    }

    private static void DrawSquare()
    {
        
    }
}