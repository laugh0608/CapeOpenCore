# Repository Guidelines

## Project Structure & Module Organization
CapeOpenCore.sln 汇集三个主要项目：`CapeOpenCore` 控制台注册器（入口 `Program.cs`，负责调用 `RegistrationServices` 注册/反注册组件）、`CapeOpenCore.Class` 核心类库（接口封装、IDL 映射及 `Resources/` 与 `StaticFiles/` 中的注册模板）、`CapeOpenCore.Test` 示例/测试装配件（`MixerExample`、截图位于 `TestScreenshot/`）。`docs/` 存放额外手册，`CapeOpen.BackUp/` 是参考备份，除非需要回溯历史，不要修改。

## Build, Test, and Development Commands
- `dotnet restore CapeOpenCore.sln`：恢复 NuGet 依赖，确保三项目引用完整。
- `msbuild CapeOpenCore.sln /p:Configuration=Debug /p:Platform=x64`：调试构建，保持与 COM 注册要求一致。
- `msbuild CapeOpenCore.sln /p:Configuration=Release /p:Platform=AnyCPU`：生成发布二进制，用于签名与交付。
- `dotnet test CapeOpenCore.Test/CapeOpenCore.Test.csproj -c Debug -p:Platform=x64`：编译并执行示例模块的集成测试钩子，及早发现 API 破坏。
- `./CapeOpenCore/bin/Debug/CapeOpenCore.exe`（管理员）: 通过菜单 1/2 注册或反注册 `CapeOpenCore.Class.dll` 与 `CapeOpenCore.Test.dll`。

## Coding Style & Naming Conventions
- 统一 4 空格缩进，文件作用域命名空间（`namespace Foo;`）优先；`using` 区块保持 System→库→项目顺序。
- 公共类型、属性使用 PascalCase，接口加 `I` 前缀，字段如需保留使用 `_camelCase`。参数和局部变量使用 camelCase，避免匈牙利命名。
- 公共 API 均添加 `/// <summary>` 文档注释，必要时提供中英文翻译，与现有 `CapeOpen*Attribute` 注释保持一致。
- COM 特性（`Guid`、`ComVisible`、自定义 `Cape*Attribute`）靠近类声明，保持清晰齐行；IDL 映射文件沿用 `*IDL.cs` 模式。
- 提交前运行 `dotnet format`（如已安装）整理 using 与空白，并确保 `LangVersion` 默认即可编译。

## Testing Guidelines
`CapeOpenCore.Test` 扮演示例模块，也容纳面向 COM 的冒烟测试。新增类命名推荐 `FeatureNameExample` 或 `FeatureNameSpec`，并放于根目录以便注册器自动加载。完成实现后：运行 `dotnet test` 确认可构建；以管理员身份运行注册器菜单 1 注册，再在目标 PME 中手动验证；将关键界面截图放入 `TestScreenshot/`，命名 `yyyyMMddHHmmss.png`。若引入标准单元测试框架（MSTest/NUnit），请放在该项目中并采用 `Should_` 风格描述行为。

## Commit & Pull Request Guidelines
- 提交信息遵循 `<type>: <summary>`（例如 `fix: 修正注册延迟`、`docs: 补充混合器说明`），单行控制在 50 字符内。
- 每个 PR 描述目的、关键修改点、影响的项目（Core/Class/Test），附上 `dotnet test` 输出摘要及必要截图或日志，引用关联 Issue（`Fixes #123`）。
- 视觉或交互层面的变更需同步更新 `TestScreenshot/`，涉及注册流程的改动必须写明需要管理员权限的步骤。

## Security & Configuration Tips
- 注册/反注册依赖管理员权限与 64 位环境；在调试机上先关闭 PME 后运行 `CapeOpenCore.exe`，避免 COM 注册锁。
- 不要随意调整 `COGuids.cs` 中的 GUID，若新增 GUID 请记录来源并更新相应的注册脚本。
- 保持 `CapeOpenCore.Class.dll` 与 `CapeOpenCore.Test.dll` 与可执行文件位于同一目录，否则 `RegistrationServices` 会抛出 `FileNotFoundException`。
