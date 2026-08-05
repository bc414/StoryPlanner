using System.Windows.Controls;

namespace WindowedStoryPlanner;

/// <summary>
/// The shared subject card, used identically by the board's grids and its tree. No code-behind
/// behaviour: opening the subject is <see cref="SubjectCardViewModel.OpenCommand"/>, supplied by
/// whichever view built the card.
/// </summary>
public partial class SubjectCardView : UserControl
{
    public SubjectCardView() => InitializeComponent();
}
