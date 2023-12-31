const { spawnSync } = require("child_process");
const fs = require("fs");

const TOTAL_COMMITS = 50;

function spacedDates2024() {
  const start = new Date("2024-01-01T12:00:00");
  const end = new Date("2024-12-31T12:00:00");
  const step = (end.getTime() - start.getTime()) / TOTAL_COMMITS;
  const dates = [];
  for (let i = 0; i < TOTAL_COMMITS; i++) {
    // để tự nhiên hơn, mỗi commit lệch ngẫu nhiên ±2 ngày
    const base = start.getTime() + step * i;
    const randomShift = (Math.random() - 0.5) * 4 * 24 * 60 * 60 * 1000;
    dates.push(new Date(base + randomShift));
  }
  return dates;
}

function formatGitDate(date) {
  const pad = n => String(n).padStart(2, "0");
  const Y = date.getFullYear();
  const M = pad(date.getMonth() + 1);
  const D = pad(date.getDate());
  const h = pad(date.getHours());
  const m = pad(date.getMinutes());
  const s = pad(date.getSeconds());
  const offset = -date.getTimezoneOffset();
  const sign = offset >= 0 ? "+" : "-";
  const hh = pad(Math.floor(Math.abs(offset) / 60));
  const mm = pad(Math.abs(offset) % 60);
  return `${Y}-${M}-${D}T${h}:${m}:${s} ${sign}${hh}${mm}`;
}

const dates = spacedDates2024();

for (let i = 1; i <= TOTAL_COMMITS; i++) {
  const d = dates[i - 1];
  const gitDate = formatGitDate(d);
  console.log(`Commit ${i}: ${gitDate}`);

  const content = `Commit ${i} at ${gitDate}\n`;
  fs.appendFileSync("history.txt", content);

  spawnSync("git", ["add", "."], { stdio: "inherit" });
  spawnSync("git", ["commit", "-m", `Evenly spaced commit ${i} in 2024`], {
    stdio: "inherit",
    env: {
      ...process.env,
      GIT_AUTHOR_DATE: gitDate,
      GIT_COMMITTER_DATE: gitDate,
    },
  });
}
