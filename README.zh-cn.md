## 概览
通过这个第三方的可编程库，探索 Shapz2 的图形和流体处理挑战，提供定制化和灵活的方法来增强你对 Shapz2 机制的理解和操控。

## 定制性
- **层数定制**：设置图形的最大层数，常见的配置有普通模式（4层）、疯狂模式（5层），也可以设置为其他层数，如10层。
- **模式支持**：库支持四分和六分模式。每层的部件数和机器基于部件数的处理方式在本库中是分离的。虽然更改 `Shape.CurrentMode` 属性通常同时调整两者，但库还提供了 `UnsafeXXX` 方法用于单独修改（详见文档注释）。

## 主要内容
此库包括：
1. Shape 和 Piece 类
2. 图形与流体处理函数
3. 图形代码合法性检验函数

## 重载特性
- **运算符重载**：`==` 和 `!=` 用于内容相等比较而非引用相等。
- **隐式转换和 ToString**：`Shape` 与 `string` 之间的隐式转换，以及 `Shape` 的 `.ToString()` 重载。
- **扩展方法**：`int` 类型的 `.CW()` 和 `.CCW()` 简化索引旋转（详见 `Shapez2.cs`），以及 `string` 的 `.ToShape()` 和 `.ToPiece()`。

## 函数版本
本库中的每个函数通常有两个版本：
1. **静态版本**：参数作为输入，返回值作为输出，输入参数保持不变。
2. **非静态版本**：通常修改调用它的实例或参数（详见文档注释），可能有或没有返回值，但性能优于静态版本。

## 使用场景
1. **学习 Shapz2 图形处理**：Shapz2 图形处理的详细实现可能在游戏 wiki 中没有完全精确的描述，但本库提供了精确的描述。
2. **图形可制作性的判别及制作方法的机械求解**：用户可以在此库的基础上实现相关求解算法。

## 注意事项
1. 当前代码更适合学习，而非高性能场景。
2. 部分非法图形的处理与原版游戏不同。例如，`Cu------` 和 `Cu------:--------` 在原版游戏中被视为不同图形，分别经过`晶体生成器`处理后生成 `Cucrcrcr` 和 `Cucrcrcr:crcrcrcr`。在本库中，它们被视为同一图形 `Cu------`。但不用担心，`Cu------:--------` 在非作弊情况下不可能在原版游戏中获得。
