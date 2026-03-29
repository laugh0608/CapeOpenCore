# CapeOpenCore .NET 10 迁移计划

更新时间：2026-03-29

## 目标

将当前仓库从 `.NET Framework 4.8` 迁移为仅保留 `.NET 10` 的实现，并继续满足现有 CAPE-OPEN/COM 使用场景。

本计划默认的落地目标是：

- 目标框架使用 `net10.0-windows`
- 发布位数固定为 `win-x64`
- 保留 Windows COM 互操作能力
- 不再产出 `net48` 或任何 .NET Framework 目标

本计划不把“跨平台运行”作为本次迁移目标。当前代码大量依赖 WinForms、COM、注册表和 Windows 部署模型，因此“去 .NET Framework”与“去 Windows 依赖”是两个不同任务，不能合并理解。

## 当前现状

### 当前框架与项目系统

- `CapeOpenCore/CapeOpenCore.csproj` 仍为旧式项目，目标框架为 `.NET Framework 4.8`
- `CapeOpenCore.Class/CapeOpenCore.Class.csproj` 仍为旧式项目，目标框架为 `.NET Framework 4.8`
- `CapeOpenCore.Test/CapeOpenCore.Test.csproj` 仍为旧式项目，目标框架为 `.NET Framework 4.8`
- `CapeOpenCore/App.config` 仍显式声明 `.NET Framework 4.8`

### 已确认的迁移约束

- 控制台注册器当前依赖 `RegistrationServices.RegisterAssembly/UnregisterAssembly`
- 类库当前通过 `[ComRegisterFunction]` / `[ComUnregisterFunction]` 直接补写 CAPE-OPEN 注册表项
- 核心类库包含 WinForms 编辑器与消息框逻辑
- 类型发现与对象创建仍大量依赖 `AppDomain`、`AssemblyResolve`、`Assembly.LoadFrom`、`CreateInstanceAndUnwrap`
- 部分包装器和异常类仍保留旧式序列化构造与 `ISerializable`

### 受影响的关键文件

- `CapeOpenCore/CapeOpenCore.csproj`
- `CapeOpenCore/Program.cs`
- `CapeOpenCore/App.config`
- `CapeOpenCore.Class/CapeOpenCore.Class.csproj`
- `CapeOpenCore.Class/CapeObjectBase.cs`
- `CapeOpenCore.Class/CapeUnitBase.cs`
- `CapeOpenCore.Class/UnitOperationSystem.cs`
- `CapeOpenCore.Class/UnitOperationManager.cs`
- `CapeOpenCore.Class/Exceptions01.cs`
- `CapeOpenCore.Class/Exceptions02.cs`
- `CapeOpenCore.Class/CapeOpenUI/BaseUnitEditor.cs`
- `CapeOpenCore.Class/CapeOpenUI/WARalgorithm.cs`
- `CapeOpenCore.Test/CapeOpenCore.Test.csproj`

## 迁移原则

1. 先保功能，再做结构优化。
2. 先完成“纯 .NET 10 可构建、可注册、可被 PME 发现”，再做体验改进。
3. 不在同一轮里同时推进“去 .NET Framework”和“跨平台 UI/跨平台 COM 替代”。
4. 保持 GUID、ProgID、CAPE-OPEN 类别注册语义稳定，避免破坏既有 PME 识别逻辑。
5. 明确区分“必须迁移项”和“可选优化项”，防止范围膨胀。

## 分阶段计划

## 阶段 0：迁移前决策

### 目标

在开始改代码前，先确定迁移边界，避免后续返工。

### 任务

- 确认最终目标框架为 `net10.0-windows`
- 确认最终发布位数为 `win-x64`
- 确认是否继续保留 WinForms 编辑器
- 确认目标 PME 是否依赖 TLB 早绑定
- 确认是否必须兼容现有注册路径与现有 ProgID

### 产出

- 一份冻结的迁移范围说明
- 一份 PME 兼容性约束清单

### 风险

- 如果 PME 依赖 TLB，而迁移时未提前确认，则 COM 暴露方案需要额外补做类型库工作

## 阶段 1：项目系统迁移

### 目标

把三个项目从旧式 `csproj` 转为 SDK-style，并切换到 `.NET 10`。

### 任务

- 将 `CapeOpenCore.csproj` 改为 SDK-style 控制台项目
- 将 `CapeOpenCore.Class.csproj` 改为 SDK-style 类库项目
- 将 `CapeOpenCore.Test.csproj` 改为 SDK-style 类库或测试项目
- 将 `TargetFrameworkVersion` 改为 `TargetFramework`，统一为 `net10.0-windows`
- 为需要 WinForms 的项目保留 `UseWindowsForms=true`
- 统一移除旧式 `<Reference Include="System"...>` 等框架引用
- 处理 `AssemblyInfo.cs` 与 SDK 自动生成程序集属性的冲突
- 删除 `App.config` 中仅服务于 .NET Framework 的 `supportedRuntime`
- 删除 binding redirects 等仅服务于 .NET Framework 的配置

### 验收标准

- `dotnet restore` 可执行
- `dotnet build` 能生成三个项目
- 没有 `.NET Framework` 专属项目属性残留

### 备注

这一步的目标只是“成功迁到 SDK-style + .NET 10 编译入口”，不要求 COM 注册立即可用。

## 阶段 2：COM 暴露与注册机制迁移

### 目标

用现代 .NET 的 COM host 模式替代 `RegistrationServices`。

### 任务

- 在 COM 类库项目中启用 `EnableComHosting`
- 评估是否需要 `EnableRegFreeCom`
- 调整注册器 `CapeOpenCore/Program.cs`
- 移除 `RegistrationServices.RegisterAssembly/UnregisterAssembly`
- 改为注册和反注册 `*.comhost.dll`
- 保留管理员提权流程，但重写执行逻辑与提示文本
- 重新验证 `[ComRegisterFunction]` / `[ComUnregisterFunction]` 在新注册流程下的调用时机
- 清理对 `InprocServer32/CodeBase` 的旧式写入逻辑
- 保留 CAPE-OPEN 所需的 `Implemented Categories` 与 `CapeDescription` 注册表补写逻辑

### 验收标准

- 生成产物中包含 `comhost` 相关文件
- 管理员执行注册后，目标组件可在注册表中被正确发现
- 反注册能完整清理本项目写入的注册信息
- PME 能枚举到测试组件

### 风险

- 当前 `CapeUnitBase.RegisterFunction` 明显建立在旧式 `RegAsm/RegistrationServices` 注册表结构之上，不能机械迁移

## 阶段 3：运行时与加载模型改造

### 目标

移除对旧 CLR/AppDomain 行为的强耦合，降低 .NET 10 下的兼容风险。

### 任务

- 将 `CreateInstanceAndUnwrap` 改为 `Activator.CreateInstance` 或明确的工厂逻辑
- 重新审视 `AppDomain.CurrentDomain.AssemblyResolve`
- 检查目录扫描与程序集加载逻辑，必要时迁到 `AssemblyLoadContext`
- 重新验证 `Assembly.LoadFrom` 的使用场景
- 重写任何依赖 remoting 语义的代码路径
- 重新测试 `UnitOperationSystem` 的“扫描 + 解析 + 实例化”流程

### 重点文件

- `CapeOpenCore.Class/CapeObjectBase.cs`
- `CapeOpenCore.Class/UnitOperationSystem.cs`
- `CapeOpenCore.Class/UnitOperationManager.cs`

### 验收标准

- 运行时不依赖 remoting 风格 API
- 本地程序集扫描与实例化在 .NET 10 下行为稳定
- 单元操作包装器能正确创建 .NET 组件实例与 COM 组件实例

## 阶段 4：序列化与异常体系清理

### 目标

移除旧式序列化兼容代码，解决 .NET 8+ 下的过时 API 问题。

### 任务

- 评估 `UnitOperationWrapper` 是否仍需要 `ISerializable`
- 如果不需要，删除 `ISerializable` 与相关构造函数
- 如果需要持久化，改用显式状态模型而不是旧式异常/对象序列化
- 清理异常类中的 `SerializationInfo/StreamingContext` 构造函数
- 解决 `SYSLIB0051` 类问题

### 验收标准

- 无必须处理的序列化过时警告
- 包装器与异常体系在 .NET 10 下可正常编译
- 持久化行为有明确的替代方案或明确弃用说明

## 阶段 5：UI 与核心逻辑边界整理

### 目标

在不影响迁移主线的前提下，降低 WinForms 对核心逻辑的耦合。

### 任务

- 保证 `BaseUnitEditor` 与 `WARalgorithm` 在 `net10.0-windows` 下可用
- 评估是否将 WinForms 编辑器从核心类库中拆分
- 将 `MessageBox.Show` 这类直接 UI 反馈替换为更可控的错误/通知策略
- 对 `RealParameter`、`IntegerParameter`、`War` 等类中的 UI 依赖点做清点

### 验收标准

- 现有编辑器仍可打开
- UI 逻辑不会阻塞核心 COM 注册与 PME 调用
- 核心库与 UI 的边界比迁移前更清晰

### 备注

这一步不是“完成纯 .NET 10 迁移”的前置条件，但建议在主线稳定后尽快处理。

## 阶段 6：部署、文档与回归验证

### 目标

确保迁移后的版本可被团队稳定构建、注册和分发。

### 任务

- 更新根目录 `README.md` 与 `docs/README.md`
- 更新构建命令，从 `msbuild + net48` 切换到 `dotnet build/publish + net10`
- 更新注册步骤、目录要求与管理员权限说明
- 明确 framework-dependent COM 部署要求
- 更新截图与示例验证说明
- 用 `CapeOpenCore.Test/MixerExample.cs` 完成最小功能回归

### 验收标准

- 新同事按照文档可完成构建与注册
- 示例混合器可在目标 PME 中被发现、加载和执行
- 文档中不再出现 `.NET Framework 4.8` 作为当前主线描述

## 关键技术决策

## 决策 1：目标框架

采用 `net10.0-windows`，不采用裸 `net10.0`。

原因：

- 类库使用 WinForms
- 运行模型依赖 Windows COM
- 注册与部署围绕 Windows 注册表展开

## 决策 2：注册方案

采用 `.NET COM host + regsvr32`，不再采用 `RegistrationServices`。

原因：

- 这是现代 .NET 对 COM 暴露的主路径
- 当前注册器逻辑依赖 .NET Framework 专属能力

## 决策 3：部署模式

优先采用 framework-dependent 部署。

原因：

- COM host 场景下更贴近官方支持路径
- 能减少自包含发布带来的额外复杂度

## 风险清单

| 风险 | 说明 | 应对策略 |
| --- | --- | --- |
| TLB 依赖未确认 | 某些 PME 可能需要类型库做早绑定 | 在阶段 0 提前验证，必要时补充 IDL/MIDL 流程 |
| 注册表结构变化 | 旧代码默认认为 `InprocServer32` 由托管程序集注册生成 | 迁移注册器时同步重写 CAPE-OPEN 补写逻辑 |
| AppDomain 行为差异 | 旧代码中存在大量 remoting 风格 API | 在阶段 3 集中替换并做专项回归 |
| 序列化 API 过时 | 旧式异常/包装器序列化会触发 .NET 8+ 警告 | 在阶段 4 统一清理 |
| 文档与实际脱节 | 现有文档仍以 net48/RegistrationServices 为核心 | 在阶段 6 统一更新 |

## 建议实施顺序

1. 完成阶段 0 决策冻结
2. 完成阶段 1 项目系统迁移并确保可编译
3. 完成阶段 2 COM 注册路径替换
4. 完成阶段 3 运行时加载模型改造
5. 完成阶段 4 序列化清理
6. 完成阶段 6 文档与 PME 回归
7. 视时间安排推进阶段 5 的 UI 解耦

## Definition of Done

满足以下条件即可判定“已彻底摒弃 .NET Framework，只保留 .NET 10”：

- 仓库内三个主项目均只面向 `.NET 10`
- 仓库中不再存在 `TargetFrameworkVersion=v4.8`
- 不再使用 `RegistrationServices`
- 不再依赖 `App.config` 的 `.NET Framework` 运行时声明
- 示例组件可被注册、发现、实例化并在目标 PME 中运行
- 构建、注册、验证文档全部更新为新流程

## 非目标

以下内容不属于本轮迁移的必做项：

- Linux/macOS 运行支持
- 非 COM 的全新插件模型替换
- 全量 UI 重写
- Rust 性能扩展落地
- 全仓库架构重构

## 建议下一步

建议直接按以下顺序开工：

1. 先改三个 `csproj` 到 SDK-style，并统一切换到 `net10.0-windows`
2. 再重写 `CapeOpenCore/Program.cs`，把注册器迁到 COM host 方案
3. 然后集中处理 `CapeObjectBase`、`CapeUnitBase`、`UnitOperationSystem` 中的旧注册与加载假设

