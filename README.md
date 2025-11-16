# CapeOpenCore

CapeOpenCore 是一个以 C# 为主的 CAPE-OPEN 组件开发与注册示例仓库，包含 CLI 注册器、核心类库以及混合器测试工程，可用于在流程模拟软件中验证自定义单元与物性包。

## 仓库概览

- `CapeOpenCore/`：CLI 注册器，提供安装、注册与反注册流程，需要 Windows 管理员权限。
- `CapeOpenCore.Class/`：主要 CAPE-OPEN 类库，纯 C# 实现，不依赖官方 C++ DLL，可直接与流程模拟软件进行 COM 互操作。
- `CapeOpenCore.Test/`：混合器示例/测试项目，记录验证截图并将在未来拆分为独立示例。
- `CapeOpen.BackUp/`：官方 C++ 物性包与混合器示例，以及 `wbarret1` 的 C# 混合器项目，仅作历史和技术参考，尽量不要修改。
- `docs/`：包含仓库结构、构建流程与规划的详细说明。

## 构建与测试

```bash
dotnet restore CapeOpenCore.sln
msbuild CapeOpenCore.sln /p:Configuration=Debug /p:Platform=x64
dotnet test CapeOpenCore.Test/CapeOpenCore.Test.csproj -c Debug -p:Platform=x64
```

如需发布构建，请改用 `Release/AnyCPU` 配置。成功构建后运行 `./CapeOpenCore/bin/Debug/CapeOpenCore.exe`，通过菜单完成 `CapeOpenCore.Class.dll` 与 `CapeOpenCore.Test.dll` 的注册或反注册。

## 当前状态与规划

- 核心类库主要基于对 `wbarret1` C# 混合器示例的翻译与重构（.NET Framework 4.8，依赖 Windows API）。
- 计划引入新的 .NET 10 库以获得跨平台能力，并逐步加入 Rust 扩展模块提升计算性能。
- 示例项目将逐步拆分并覆盖更多 CAPE-OPEN 场景，配套文档可在 `docs/` 中查阅。

更多细节与贡献指南请参考 `docs/README.md`。
