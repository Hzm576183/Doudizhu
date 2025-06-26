# 斗地主游戏构建说明

## 快速构建

### 方法一：使用Unity编辑器构建菜单
1. 在Unity中打开项目
2. 顶部菜单选择 `Build` → `Build Doudizhu Game`
3. 自动构建当前平台版本到 `Builds/` 文件夹

### 方法二：构建所有平台版本
1. 在Unity中选择 `Build` → `Build All Platforms`
2. 自动构建Windows、Mac、Linux三个版本

### 方法三：传统Unity构建方式
1. `File` → `Build Settings`
2. 点击 `Add Open Scenes` 添加当前场景
3. 选择目标平台 (Windows/Mac/Linux)
4. 点击 `Build` 选择输出文件夹

## 游戏功能

✅ **完整的斗地主游戏**
- 54张牌完整牌库（含大小王）
- 标准斗地主规则和牌型识别
- 智能AI对手
- 可视化界面和交互

✅ **游戏操作**
- 鼠标点击选牌
- 出牌/不出按钮
- 键盘快捷键支持

✅ **跨平台支持**
- Windows (64位)
- macOS (Intel/M1)
- Linux (64位)

## 构建输出

构建完成后，在 `Builds/` 文件夹中会找到：

```
Builds/
├── Windows/
│   ├── DouDiZhu.exe           # Windows可执行文件
│   └── DouDiZhu_Data/         # 游戏数据文件夹
├── Mac/
│   └── DouDiZhu.app           # Mac应用程序包
└── Linux/
    ├── DouDiZhu               # Linux可执行文件
    └── DouDiZhu_Data/         # 游戏数据文件夹
```

## 分发说明

### Windows版本
- 分发整个 `Windows/` 文件夹
- 用户双击 `DouDiZhu.exe` 运行

### Mac版本  
- 分发 `DouDiZhu.app` 文件
- 用户双击运行（可能需要在安全设置中允许）

### Linux版本
- 分发整个 `Linux/` 文件夹
- 用户在终端中运行：`chmod +x DouDiZhu && ./DouDiZhu`

## Steam发布准备

如果要发布到Steam：

1. **游戏构建**：使用上述方法构建各平台版本
2. **Steam SDK集成**：需要添加Steam SDK支持
3. **成就系统**：可以添加Steam成就
4. **云存档**：集成Steam云存档功能

## 技术细节

- **Unity版本**：6000.0.51f1
- **渲染管线**：内置渲染管线
- **UI系统**：OnGUI (即时模式GUI)
- **脚本语言**：C#
- **最小系统要求**：
  - Windows 10+ / macOS 10.14+ / Ubuntu 18.04+
  - 1GB RAM
  - 500MB存储空间

## 故障排除

**构建失败**：
- 确保场景已保存
- 检查Console窗口的错误信息
- 重新导入所有资源：`Assets` → `Reimport All`

**游戏无法运行**：
- 确保所有必要文件都在同一文件夹中
- Windows用户可能需要安装Visual C++运行库
- Mac用户可能需要在系统偏好设置中允许运行