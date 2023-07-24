using Silk.NET.Input;
using Silk.NET.Maths;
using Silk.NET.Windowing;
using Silk.NET.OpenGL;

public class SilkWindow
{
    private static IWindow _window;
    private static GL gl;
    private static uint program;

    public SilkWindow()
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
    
    private static readonly string VertexShaderSource = @"
        #version 330 core
        layout (location = 0) in vec3 vPos;
		layout (location = 1) in vec4 vCol;

		out vec4 outCol;
        
        void main()
        {
			outCol = vCol;
            gl_Position = vec4(vPos.x, vPos.y, vPos.z, 1.0);
        }
        ";


    private static readonly string FragmentShaderSource = @"
        #version 330 core
        out vec4 FragColor;
		
		in vec4 outCol;

        void main()
        {
            FragColor = outCol;
        }
        ";
    
    
    

    private static void OnLoad()
    {
        IInputContext input = _window.CreateInput();
        gl = _window.CreateOpenGL();
        gl.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);

        // Console.WriteLine("Load!");
        
        
        
        uint vshader = gl.CreateShader(ShaderType.VertexShader);
        uint fshader = gl.CreateShader(ShaderType.FragmentShader);
        gl.ShaderSource(vshader, VertexShaderSource);
        gl.ShaderSource(fshader, VertexShaderSource);
        gl.CompileShader(vshader);
        gl.CompileShader(fshader);

        program = gl.CreateProgram();
        gl.AttachShader(program,vshader);
        gl.AttachShader(program,fshader);
        
        gl.LinkProgram(program);
        
        gl.DetachShader(program,vshader);
        gl.DetachShader(program,fshader);
        
        gl.DeleteShader(vshader);
        gl.DeleteShader(fshader);
        
        //Input
        for (int i = 0; i < input.Keyboards.Count; i++)
            input.Keyboards[i].KeyDown += KeyDown;
    }

    private static void OnUpdate(double deltaTime)
    {
        // Console.WriteLine("Update!");
    }

    private static unsafe void OnRender(double deltaTime)
    {
        gl.Clear(ClearBufferMask.ColorBufferBit);
        
        uint vao = gl.GenVertexArray();
        gl.BindVertexArray(vao);
        
        uint vertices =  gl.GenBuffer();
        uint colors = gl.GenBuffer();
        uint indices = gl.GenBuffer();


        float[] vertexArray = new float[]
        {
            -0.5f, -0.5f , 0.0f, 
            +0,5f, -0.5f, 0.0f,
            0.0f, +0.5f, 0.0f
        };

        float[] colorArray = new float[]
        {
            1.0f,0.0f,0.0f, 1.0f,
            0.0f, 1.0f, 0.0f, 1.0f,
            0.0f, 1.0f, 0.0f, 1.0f
        };

        uint[] indexArray = new uint[]
        {
            0,1,2
        }; 
        
        gl.BindBuffer(GLEnum.ArrayBuffer,vertices);
        gl.BufferData(GLEnum.ArrayBuffer, (ReadOnlySpan<float>)vertexArray.AsSpan(), GLEnum.StaticDraw);
        gl.VertexAttribPointer(0,3,GLEnum.Float,false,0, null);
        gl.EnableVertexAttribArray(0);
        
        gl.BindBuffer(GLEnum.ArrayBuffer,colors);
        gl.BufferData(GLEnum.ArrayBuffer, (ReadOnlySpan<float>)colorArray.AsSpan(), GLEnum.StaticDraw);
        gl.VertexAttribPointer(1,3,GLEnum.Float,false,0, null);
        gl.EnableVertexAttribArray(1);
        
        gl.BindBuffer(GLEnum.ElementArrayBuffer,indices);
        gl.BufferData(GLEnum.ElementArrayBuffer, (ReadOnlySpan<uint>)indexArray.AsSpan(), GLEnum.StaticDraw);
        
        //Binding arrayBuffer to 0 unbinds it
        gl.BindBuffer(GLEnum.ArrayBuffer,0);
        //If null uses bound element array
        gl.DrawElements(GLEnum.Triangles,1, GLEnum.UnsignedInt, null);
        
        gl.BindBuffer(GLEnum.ElementArrayBuffer,0);
        gl.BindVertexArray(vao);
        
        gl.DeleteBuffer(vertices);
        gl.DeleteBuffer(indices);
        gl.DeleteBuffer(indices);
        gl.DeleteVertexArray(vao);

    }

    private static void KeyDown(IKeyboard keyboard, Key key, int keyCode)
    {
        if (key == Key.Escape)
            _window.Close();
    }
}