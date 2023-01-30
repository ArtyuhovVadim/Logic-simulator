using LogicSimulator.ViewModels.EditorViewModels.Base;

namespace LogicSimulator.ViewModels.EditorViewModels.Layout.Builders;

public class LayoutBuilder
{
    private readonly EditorLayout _layoutViewModel;

    private LayoutBuilder(EditorViewModel editorViewModel) => _layoutViewModel = new EditorLayout
    {
        ObjectName = string.Empty,
        EditorViewModel = editorViewModel
    };

    public static LayoutBuilder Create(EditorViewModel editorViewModel) => new(editorViewModel);

    public LayoutBuilder WithName(string name)
    {
        _layoutViewModel.ObjectName = name;
        return this;
    }

    public LayoutBuilder WithGroup(Action<GroupBuilder> configureAction)
    {
        var groupBuilder = new GroupBuilder(_layoutViewModel.EditorViewModel);
        configureAction.Invoke(groupBuilder);
        _layoutViewModel.Groups.Add(groupBuilder.Build());
        return this;
    }

    public EditorLayout Build() => _layoutViewModel;
}