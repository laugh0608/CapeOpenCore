# CapeOpenCore 文档索引

本文档概述当前仓库的组织结构、各项目的职责以及后续扩展计划，帮助贡献者快速了解维护重点与开发路线。

## 仓库结构

| 路径 | 说明 |
| --- | --- |
| `CapeOpenCore/` | 整个工具链的入口。提供 CLI 安装界面，负责调用 `RegistrationServices` 完成 CAPE-OPEN 组件的注册/反注册。 |
| `CapeOpenCore.Class/` | 主要的 CAPE-OPEN 类库，纯 C# 实现，不依赖官方 C++ 编译的 `CAPE-OPENv1-1-0.dll`，包含接口封装、IDL 映射、注册模板等。 |
| `CapeOpenCore.Test/` | 当前的混合器示例/测试项目。后续计划拆分为独立示例工程。 |
| `CapeOpen.BackUp/` | 官方 C++ 物性包与混合器示例，以及 `wbarret1` 的 C# 混合器参考代码。仅作历史及参考用途，请勿随意修改。 |
| `docs/` | 仓库文档集合（本文件）。 |

## 项目职责

### CapeOpenCore
* 以 CLI 菜单提供注册（菜单项 1）与反注册（菜单项 2）流程。
* 要求 Windows 管理员权限以及与 `CapeOpenCore.Class.dll`、`CapeOpenCore.Test.dll` 同目录运行。
* 配置示例：
  ```bash
  dotnet restore CapeOpenCore.sln
  msbuild CapeOpenCore.sln /p:Configuration=Debug /p:Platform=x64
  ./CapeOpenCore/bin/Debug/CapeOpenCore.exe
  ```

### CapeOpenCore.Class
* 核心库由 C# 编写，可以直接对流程模拟软件暴露 CAPE-OPEN COM 接口。
* 当前代码基础主要来源于对 `wbarret1` C# 混合器示例的翻译与重构（.NET Framework 4.8，重度依赖 Windows API）。
* 提供 `Resources/` 与 `StaticFiles/` 中的注册模板以及大量 `*IDL.cs` 文件，用于保持与 CAPE-OPEN 规范的映射。

### CapeOpenCore.Test
* 作为混合器示例，配合 `CapeOpenCore` 注册后可在目标 PME 中验证接口行为。
* `TestScreenshot/` 保存关键界面截图，命名约定 `yyyyMMddHHmmss.png`。

### CapeOpen.BackUp
* 包含 CAPE-OPEN 官方 C++ 参考实现（物性包、混合器）与 `wbarret1` 的 C# 示例。
* 不参与直接构建，仅作调研、对照与未来扩展的素材来源。

## 构建与测试流程

1. 还原依赖：`dotnet restore CapeOpenCore.sln`
2. 调试构建：`msbuild CapeOpenCore.sln /p:Configuration=Debug /p:Platform=x64`
3. 发布构建：`msbuild CapeOpenCore.sln /p:Configuration=Release /p:Platform=AnyCPU`
4. 示例测试：`dotnet test CapeOpenCore.Test/CapeOpenCore.Test.csproj -c Debug -p:Platform=x64`

完成构建后以管理员身份运行 `CapeOpenCore.exe`，通过 CLI 完成注册，再在目标 PME 中进行人工验证。

## 未来规划

* **跨平台核心库**：在现有 .NET Framework 4.8 代码基础上，引入全新 .NET 10 项目，实现跨平台的 CAPE-OPEN 交互能力。
* **性能扩展**：计划新增 Rust 扩展模块/项目，用于提升流程模拟迭代与求解场景的性能。
* **示例解耦**：把 `CapeOpenCore.Test` 演进为多个独立示例项目，覆盖混合器、物性包等不同场景。

随着这些迭代推进，文档会同步更新，欢迎在提出 PR 前先阅读本文件与根目录 README，确保与最新架构保持一致。
