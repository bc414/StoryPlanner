# Tallies a supersession-audit run's results mechanically: relation counts, then every
# unit whose relation is one of the flagged labels, with its section, quote and note.
# Adjudication reads this, never the raw blocks.
#
#   .\tally.ps1 -Run .\2026-09-03-v3-buildout [-Flag absent,reversed,delegated,narrowed]
#
# ASCII only: Windows PowerShell 5.1 reads an unmarked file as ANSI.
param(
    [Parameter(Mandatory)] [string] $Run,
    [string[]] $Flag = @('absent', 'reversed', 'delegated')
)

$files = Get-ChildItem (Join-Path $Run 'results') -Filter *.md | Sort-Object Name
if ($files.Count -eq 0) { throw "no results under $Run" }

$blocks = @()
foreach ($f in $files) {
    $cur = $null
    foreach ($line in Get-Content $f.FullName -Encoding UTF8) {
        if ($line -match '^## (unit-\d{3})$') {
            if ($cur) { $blocks += $cur }
            $cur = [ordered]@{ Unit = $matches[1]; File = $f.Name; Section = ''; Quote = ''; Counterpart = ''; Relation = ''; Note = '' }
        }
        elseif ($cur -and $line -match '^- (section|quote|counterpart|relation|note):\s*(.*)$') {
            $key = (Get-Culture).TextInfo.ToTitleCase($matches[1])
            $cur[$key] = $matches[2]
        }
    }
    if ($cur) { $blocks += $cur }
}

"Blocks: $($blocks.Count) in $($files.Count) result files"
""
"Relation counts:"
$blocks | Group-Object { $_.Relation } | Sort-Object Count -Descending | ForEach-Object { "{0,5}  {1}" -f $_.Count, $_.Name }
""
"Flagged ($($Flag -join ', ')):"
foreach ($b in $blocks | Where-Object { $Flag -contains $_.Relation }) {
    "{0} | {1} | {2}" -f $b.Unit, $b.Relation, $b.Section
    "   quote: $($b.Quote)"
    "   note:  $($b.Note)"
}
