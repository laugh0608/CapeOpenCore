# CapeOpenCore

> 本仓库已进入封存状态，作为历史参考与 CAPE-OPEN 资产来源保留。
>
> 新的主开发仓库已迁移至：
> [RadishFlow](https://github.com/laugh0608/RadishFlow)

CapeOpenCore 是一个以 C# 为主的 CAPE-OPEN 组件开发与注册示例仓库，包含 CLI 注册器、核心类库以及混合器测试工程，可用于在流程模拟软件中验证自定义单元与物性包。

## 仓库状态

本仓库当前定位为：

- 历史参考仓库
- CAPE-OPEN 接口、GUID、异常语义与注册语义的提取来源
- `.NET Framework 4.8` / 旧式 COM 注册方案的保留样例

本仓库默认**不再承接新产品主线开发**。后续与 Rust Core、Rust UI、`.NET 10` CAPE-OPEN 适配层相关的新工作，请前往 [RadishFlow](https://github.com/laugh0608/RadishFlow)。

## 与新仓库的关系

`RadishFlow` 是新的主开发仓库，目标是：

- 使用 Rust 实现稳态模拟核心
- 使用 Rust 实现桌面 UI
- 使用 `.NET 10` 实现 CAPE-OPEN/COM 适配层
- 第一阶段只导出自有 CAPE-OPEN Unit Operation PMC 给外部 PME 使用

当前 `CapeOpenCore` 保留的主要价值是：

- `*IDL.cs` 与相关接口定义参考
- `COGuids.cs` 中的 GUID 与 CAPE-OPEN category 语义
- ECape 异常分类语义
- 旧注册器与注册表补写逻辑的历史实现参考
- `MixerExample` 等最小行为样例

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

- 核心类库主要基于对 `wbarret1` C# 混合器示例的翻译与重构（`.NET Framework 4.8`，依赖 Windows API）。
- 本仓库不再作为未来主产品演进载体，主线规划已转移至 `RadishFlow`。
- `docs/` 中保留了从 `CapeOpenCore` 过渡到 `RadishFlow` 的迁移、架构和资产提取文档，可继续作为参考。

更多细节与贡献指南请参考 `docs/README.md`。
