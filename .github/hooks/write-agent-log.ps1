param(
    [string]$EventName = "Unknown"
)

$logPath = "C:\Users\Gabriel\TrackIT\.github\hooks\agent_log.txt"
$payload = [Console]::In.ReadToEnd()

if (-not [string]::IsNullOrWhiteSpace($payload)) {
    Add-Content -Path $logPath -Value ("[{0}] {1}" -f (Get-Date -Format s), $EventName)
    Add-Content -Path $logPath -Value $payload
    Add-Content -Path $logPath -Value ""
}
