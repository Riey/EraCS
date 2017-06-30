using EraCS.Core.Test.Program;

// 빈 페이지 항목 템플릿에 대한 설명은 https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x412에 나와 있습니다.

namespace EraCS.UI.FrontEnd.Basic.Test.UWP
{
    /// <summary>
    /// 자체적으로 사용하거나 프레임 내에서 탐색할 수 있는 빈 페이지입니다.
    /// </summary>
    public sealed partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
            var program = new TestProgram();
            LoadApplication(new FrontEnd.Basic.App(program.Console));
            program.Start();
        }
    }
}
