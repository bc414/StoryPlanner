$in = [Console]::In.ReadToEnd()
$j = $in | ConvertFrom-Json
if ($j.permission_mode -eq 'plan' -and $j.tool_name -like 'mcp__storyplanner__*') {
  '{"hookSpecificOutput":{"hookEventName":"PreToolUse","permissionDecision":"allow","permissionDecisionReason":"storyplanner MCP auto-approved in plan mode"}}'
}
