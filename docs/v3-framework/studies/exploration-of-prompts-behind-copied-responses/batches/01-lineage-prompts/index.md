# 01-lineage-prompts — index

- itemizer: tools/StoryPlanner.TurnItemizer, 2026-09-27 2164cca-dirty
- narrowing: the user turns that precede a model turn in the lineage layers, one item per model turn, a user turn made only of attached documents that were never captured left out; the item carries the user turn alone, and whether the model turn's text was taken into the v1 archive — a note pasted or lifted from it, as the attribution run finds it — is in the index description only; in the layers gemini, aistudio, notebooklm; archive notes of any story named with "Paratext" not counted
- locator notation: `<model turn> → <user turn>`, each turn as the source ids of its records, joined by + when it spans several: `gemini:<entry id> prompt|response` and `aistudio:<chat id>#t<turn>` and `nlm:<notebook id>#t<turn>` in lineage.db, `block:<block id>` in the conversations tables of the working-plan .storyplan; a Gemini turn's neighbour is the entry before or after it in its thread as the Gemini corpus index groups entries; `(none)` when no user turn follows

| item | locator | description |
|---|---|---|
| gemini-2117-prompt | gemini:2117 response → gemini:2117 prompt | Gemini web (lineage), thread th_006f9379, 2026-02-25; copied: 5 pasted, 0 lifted into archive notes |
| gemini-2015-prompt | gemini:2015 response → gemini:2015 prompt | Gemini web (lineage), thread th_00990b72, 2026-02-20 to 2026-02-21; not copied |
| gemini-2017-prompt | gemini:2017 response → gemini:2017 prompt | Gemini web (lineage), thread th_00990b72, 2026-02-20 to 2026-02-21; not copied |
| gemini-2018-prompt | gemini:2018 response → gemini:2018 prompt | Gemini web (lineage), thread th_00990b72, 2026-02-20 to 2026-02-21; not copied |
| gemini-2020-prompt | gemini:2020 response → gemini:2020 prompt | Gemini web (lineage), thread th_00990b72, 2026-02-20 to 2026-02-21; not copied |
| gemini-2022-prompt | gemini:2022 response → gemini:2022 prompt | Gemini web (lineage), thread th_00990b72, 2026-02-20 to 2026-02-21; not copied |
| gemini-2024-prompt | gemini:2024 response → gemini:2024 prompt | Gemini web (lineage), thread th_00990b72, 2026-02-20 to 2026-02-21; not copied |
| gemini-2026-prompt | gemini:2026 response → gemini:2026 prompt | Gemini web (lineage), thread th_00990b72, 2026-02-20 to 2026-02-21; not copied |
| gemini-2380-prompt | gemini:2380 response → gemini:2380 prompt | Gemini web (lineage), thread th_00cb256e, 2026-03-04; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2751-prompt | gemini:2751 response → gemini:2751 prompt | Gemini web (lineage), thread th_01e188ba, 2026-03-17; not copied |
| gemini-2752-prompt | gemini:2752 response → gemini:2752 prompt | Gemini web (lineage), thread th_01e188ba, 2026-03-17; not copied |
| gemini-2753-prompt | gemini:2753 response → gemini:2753 prompt | Gemini web (lineage), thread th_01e188ba, 2026-03-17; not copied |
| gemini-2754-prompt | gemini:2754 response → gemini:2754 prompt | Gemini web (lineage), thread th_01e188ba, 2026-03-17; not copied |
| gemini-2755-prompt | gemini:2755 response → gemini:2755 prompt | Gemini web (lineage), thread th_01e188ba, 2026-03-17; not copied |
| gemini-999-prompt | gemini:999 response → gemini:999 prompt | Gemini web (lineage), thread th_0211d085, 2026-01-24; copied: 1 pasted, 1 lifted into archive notes |
| gemini-58-prompt | gemini:58 response → gemini:58 prompt | Gemini web (lineage), thread th_022a9719, 2025-11-26; not copied |
| gemini-59-prompt | gemini:59 response → gemini:59 prompt | Gemini web (lineage), thread th_022a9719, 2025-11-26; not copied |
| gemini-60-prompt | gemini:60 response → gemini:60 prompt | Gemini web (lineage), thread th_022a9719, 2025-11-26; not copied |
| gemini-2357-prompt | gemini:2357 response → gemini:2357 prompt | Gemini web (lineage), thread th_0248f7e3, 2026-03-04; not copied |
| gemini-1781-prompt | gemini:1781 response → gemini:1781 prompt | Gemini web (lineage), thread th_036f6236, 2026-02-13; not copied |
| gemini-1119-prompt | gemini:1119 response → gemini:1119 prompt | Gemini web (lineage), thread th_04185c4f, 2026-01-26; not copied |
| gemini-1120-prompt | gemini:1120 response → gemini:1120 prompt | Gemini web (lineage), thread th_04185c4f, 2026-01-26; not copied |
| gemini-2862-prompt | gemini:2862 response → gemini:2862 prompt | Gemini web (lineage), thread th_043fbe5f, 2026-03-23; not copied |
| gemini-2863-prompt | gemini:2863 response → gemini:2863 prompt | Gemini web (lineage), thread th_043fbe5f, 2026-03-23; not copied |
| gemini-2705-prompt | gemini:2705 response → gemini:2705 prompt | Gemini web (lineage), thread th_046298ed, 2026-03-16; not copied |
| gemini-622-prompt | gemini:622 response → gemini:622 prompt | Gemini web (lineage), thread th_046723ce, 2026-01-10; not copied |
| gemini-623-prompt | gemini:623 response → gemini:623 prompt | Gemini web (lineage), thread th_046723ce, 2026-01-10; not copied |
| gemini-624-prompt | gemini:624 response → gemini:624 prompt | Gemini web (lineage), thread th_046723ce, 2026-01-10; not copied |
| gemini-186-prompt | gemini:186 response → gemini:186 prompt | Gemini web (lineage), thread th_0472218d, 2025-12-04; not copied |
| gemini-187-prompt | gemini:187 response → gemini:187 prompt | Gemini web (lineage), thread th_0472218d, 2025-12-04; not copied |
| gemini-849-prompt | gemini:849 response → gemini:849 prompt | Gemini web (lineage), thread th_04b33d90, 2026-01-19; not copied |
| gemini-850-prompt | gemini:850 response → gemini:850 prompt | Gemini web (lineage), thread th_04b33d90, 2026-01-19; not copied |
| gemini-851-prompt | gemini:851 response → gemini:851 prompt | Gemini web (lineage), thread th_04b33d90, 2026-01-19; not copied |
| gemini-852-prompt | gemini:852 response → gemini:852 prompt | Gemini web (lineage), thread th_04b33d90, 2026-01-19; not copied |
| gemini-853-prompt | gemini:853 response → gemini:853 prompt | Gemini web (lineage), thread th_04b33d90, 2026-01-19; not copied |
| gemini-824-prompt | gemini:824 response → gemini:824 prompt | Gemini web (lineage), thread th_04c00e2d, 2026-01-18; not copied |
| gemini-2678-prompt | gemini:2678 response → gemini:2678 prompt | Gemini web (lineage), thread th_04d4cf82, 2026-03-13; copied: 1 pasted, 0 lifted into archive notes |
| gemini-1711-prompt | gemini:1711 response → gemini:1711 prompt | Gemini web (lineage), thread th_04f20b0b, 2026-02-11; not copied |
| gemini-94-prompt | gemini:94 response → gemini:94 prompt | Gemini web (lineage), thread th_0540a429, 2025-12-02; copied: 3 pasted, 0 lifted into archive notes |
| gemini-95-prompt | gemini:95 response → gemini:95 prompt | Gemini web (lineage), thread th_0540a429, 2025-12-02; not copied |
| gemini-96-prompt | gemini:96 response → gemini:96 prompt | Gemini web (lineage), thread th_0540a429, 2025-12-02; not copied |
| gemini-97-prompt | gemini:97 response → gemini:97 prompt | Gemini web (lineage), thread th_0540a429, 2025-12-02; copied: 2 pasted, 1 lifted into archive notes |
| gemini-98-prompt | gemini:98 response → gemini:98 prompt | Gemini web (lineage), thread th_0540a429, 2025-12-02; not copied |
| gemini-99-prompt | gemini:99 response → gemini:99 prompt | Gemini web (lineage), thread th_0540a429, 2025-12-02; not copied |
| gemini-200-prompt | gemini:200 response → gemini:200 prompt | Gemini web (lineage), thread th_05474992, 2025-12-05; not copied |
| gemini-201-prompt | gemini:201 response → gemini:201 prompt | Gemini web (lineage), thread th_05474992, 2025-12-05; copied: 2 pasted, 0 lifted into archive notes |
| gemini-202-prompt | gemini:202 response → gemini:202 prompt | Gemini web (lineage), thread th_05474992, 2025-12-05; copied: 3 pasted, 0 lifted into archive notes |
| gemini-203-prompt | gemini:203 response → gemini:203 prompt | Gemini web (lineage), thread th_05474992, 2025-12-05; copied: 1 pasted, 0 lifted into archive notes |
| gemini-1054-prompt | gemini:1054 response → gemini:1054 prompt | Gemini web (lineage), thread th_0563fb6c, 2026-01-25; not copied |
| gemini-1055-prompt | gemini:1055 response → gemini:1055 prompt | Gemini web (lineage), thread th_0563fb6c, 2026-01-25; copied: 0 pasted, 1 lifted into archive notes |
| gemini-1056-prompt | gemini:1056 response → gemini:1056 prompt | Gemini web (lineage), thread th_0563fb6c, 2026-01-25; not copied |
| gemini-1057-prompt | gemini:1057 response → gemini:1057 prompt | Gemini web (lineage), thread th_0563fb6c, 2026-01-25; not copied |
| gemini-2919-prompt | gemini:2919 response → gemini:2919 prompt | Gemini web (lineage), thread th_05701972, 2026-03-24; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2920-prompt | gemini:2920 response → gemini:2920 prompt | Gemini web (lineage), thread th_05701972, 2026-03-24; not copied |
| gemini-2134-prompt | gemini:2134 response → gemini:2134 prompt | Gemini web (lineage), thread th_06155f49, 2026-02-25; not copied |
| gemini-2135-prompt | gemini:2135 response → gemini:2135 prompt | Gemini web (lineage), thread th_06155f49, 2026-02-25; not copied |
| gemini-2136-prompt | gemini:2136 response → gemini:2136 prompt | Gemini web (lineage), thread th_06155f49, 2026-02-25; copied: 2 pasted, 0 lifted into archive notes |
| gemini-1358-prompt | gemini:1358 response → gemini:1358 prompt | Gemini web (lineage), thread th_0636962e, 2026-02-04; copied: 2 pasted, 1 lifted into archive notes |
| gemini-1359-prompt | gemini:1359 response → gemini:1359 prompt | Gemini web (lineage), thread th_0636962e, 2026-02-04; not copied |
| gemini-1360-prompt | gemini:1360 response → gemini:1360 prompt | Gemini web (lineage), thread th_0636962e, 2026-02-04; not copied |
| gemini-1361-prompt | gemini:1361 response → gemini:1361 prompt | Gemini web (lineage), thread th_0636962e, 2026-02-04; copied: 2 pasted, 0 lifted into archive notes |
| gemini-890-prompt | gemini:890 response → gemini:890 prompt | Gemini web (lineage), thread th_070fa0df, 2026-01-22; not copied |
| gemini-891-prompt | gemini:891 response → gemini:891 prompt | Gemini web (lineage), thread th_070fa0df, 2026-01-22; not copied |
| gemini-912-prompt | gemini:912 response → gemini:912 prompt | Gemini web (lineage), thread th_07f5fa9b, 2026-01-22; not copied |
| gemini-913-prompt | gemini:913 response → gemini:913 prompt | Gemini web (lineage), thread th_07f5fa9b, 2026-01-22; not copied |
| gemini-914-prompt | gemini:914 response → gemini:914 prompt | Gemini web (lineage), thread th_07f5fa9b, 2026-01-22; not copied |
| gemini-915-prompt | gemini:915 response → gemini:915 prompt | Gemini web (lineage), thread th_07f5fa9b, 2026-01-22; not copied |
| gemini-916-prompt | gemini:916 response → gemini:916 prompt | Gemini web (lineage), thread th_07f5fa9b, 2026-01-22; not copied |
| gemini-917-prompt | gemini:917 response → gemini:917 prompt | Gemini web (lineage), thread th_07f5fa9b, 2026-01-22; not copied |
| gemini-918-prompt | gemini:918 response → gemini:918 prompt | Gemini web (lineage), thread th_07f5fa9b, 2026-01-22; not copied |
| gemini-919-prompt | gemini:919 response → gemini:919 prompt | Gemini web (lineage), thread th_07f5fa9b, 2026-01-22; not copied |
| gemini-920-prompt | gemini:920 response → gemini:920 prompt | Gemini web (lineage), thread th_07f5fa9b, 2026-01-22; copied: 1 pasted, 0 lifted into archive notes |
| gemini-1665-prompt | gemini:1665 response → gemini:1665 prompt | Gemini web (lineage), thread th_0848ce06, 2026-02-10; not copied |
| gemini-1666-prompt | gemini:1666 response → gemini:1666 prompt | Gemini web (lineage), thread th_0848ce06, 2026-02-10; not copied |
| gemini-1667-prompt | gemini:1667 response → gemini:1667 prompt | Gemini web (lineage), thread th_0848ce06, 2026-02-10; not copied |
| gemini-1668-prompt | gemini:1668 response → gemini:1668 prompt | Gemini web (lineage), thread th_0848ce06, 2026-02-10; not copied |
| gemini-1669-prompt | gemini:1669 response → gemini:1669 prompt | Gemini web (lineage), thread th_0848ce06, 2026-02-10; not copied |
| gemini-1670-prompt | gemini:1670 response → gemini:1670 prompt | Gemini web (lineage), thread th_0848ce06, 2026-02-10; not copied |
| gemini-1671-prompt | gemini:1671 response → gemini:1671 prompt | Gemini web (lineage), thread th_0848ce06, 2026-02-10; not copied |
| gemini-816-prompt | gemini:816 response → gemini:816 prompt | Gemini web (lineage), thread th_0862f7b4, 2026-01-18; not copied |
| gemini-817-prompt | gemini:817 response → gemini:817 prompt | Gemini web (lineage), thread th_0862f7b4, 2026-01-18; not copied |
| gemini-818-prompt | gemini:818 response → gemini:818 prompt | Gemini web (lineage), thread th_0862f7b4, 2026-01-18; not copied |
| gemini-819-prompt | gemini:819 response → gemini:819 prompt | Gemini web (lineage), thread th_0862f7b4, 2026-01-18; not copied |
| gemini-820-prompt | gemini:820 response → gemini:820 prompt | Gemini web (lineage), thread th_0862f7b4, 2026-01-18; not copied |
| gemini-821-prompt | gemini:821 response → gemini:821 prompt | Gemini web (lineage), thread th_0862f7b4, 2026-01-18; copied: 1 pasted, 0 lifted into archive notes |
| gemini-822-prompt | gemini:822 response → gemini:822 prompt | Gemini web (lineage), thread th_0862f7b4, 2026-01-18; not copied |
| gemini-206-prompt | gemini:206 response → gemini:206 prompt | Gemini web (lineage), thread th_08dca7a6, 2025-12-05; copied: 1 pasted, 0 lifted into archive notes |
| gemini-207-prompt | gemini:207 response → gemini:207 prompt | Gemini web (lineage), thread th_08dca7a6, 2025-12-05; not copied |
| gemini-208-prompt | gemini:208 response → gemini:208 prompt | Gemini web (lineage), thread th_08dca7a6, 2025-12-05; not copied |
| gemini-209-prompt | gemini:209 response → gemini:209 prompt | Gemini web (lineage), thread th_08dca7a6, 2025-12-05; not copied |
| gemini-210-prompt | gemini:210 response → gemini:210 prompt | Gemini web (lineage), thread th_08dca7a6, 2025-12-05; copied: 1 pasted, 0 lifted into archive notes |
| gemini-211-prompt | gemini:211 response → gemini:211 prompt | Gemini web (lineage), thread th_08dca7a6, 2025-12-05; copied: 1 pasted, 0 lifted into archive notes |
| gemini-3095-prompt | gemini:3095 response → gemini:3095 prompt | Gemini web (lineage), thread th_08e36225, 2026-04-08; not copied |
| gemini-3089-prompt | gemini:3089 response → gemini:3089 prompt | Gemini web (lineage), thread th_094ca65e, 2026-04-07; not copied |
| gemini-2573-prompt | gemini:2573 response → gemini:2573 prompt | Gemini web (lineage), thread th_0971e3d0, 2026-03-09; not copied |
| gemini-2574-prompt | gemini:2574 response → gemini:2574 prompt | Gemini web (lineage), thread th_0971e3d0, 2026-03-09; not copied |
| gemini-508-prompt | gemini:508 response → gemini:508 prompt | Gemini web (lineage), thread th_098a2683, 2026-01-09; not copied |
| gemini-509-prompt | gemini:509 response → gemini:509 prompt | Gemini web (lineage), thread th_098a2683, 2026-01-09; not copied |
| gemini-510-prompt | gemini:510 response → gemini:510 prompt | Gemini web (lineage), thread th_098a2683, 2026-01-09; not copied |
| gemini-511-prompt | gemini:511 response → gemini:511 prompt | Gemini web (lineage), thread th_098a2683, 2026-01-09; not copied |
| gemini-512-prompt | gemini:512 response → gemini:512 prompt | Gemini web (lineage), thread th_098a2683, 2026-01-09; not copied |
| gemini-513-prompt | gemini:513 response → gemini:513 prompt | Gemini web (lineage), thread th_098a2683, 2026-01-09; not copied |
| gemini-514-prompt | gemini:514 response → gemini:514 prompt | Gemini web (lineage), thread th_098a2683, 2026-01-09; not copied |
| gemini-515-prompt | gemini:515 response → gemini:515 prompt | Gemini web (lineage), thread th_098a2683, 2026-01-09; not copied |
| gemini-516-prompt | gemini:516 response → gemini:516 prompt | Gemini web (lineage), thread th_098a2683, 2026-01-09; not copied |
| gemini-517-prompt | gemini:517 response → gemini:517 prompt | Gemini web (lineage), thread th_098a2683, 2026-01-09; not copied |
| gemini-518-prompt | gemini:518 response → gemini:518 prompt | Gemini web (lineage), thread th_098a2683, 2026-01-09; not copied |
| gemini-519-prompt | gemini:519 response → gemini:519 prompt | Gemini web (lineage), thread th_098a2683, 2026-01-09; not copied |
| gemini-520-prompt | gemini:520 response → gemini:520 prompt | Gemini web (lineage), thread th_098a2683, 2026-01-09; not copied |
| gemini-521-prompt | gemini:521 response → gemini:521 prompt | Gemini web (lineage), thread th_098a2683, 2026-01-09; not copied |
| gemini-522-prompt | gemini:522 response → gemini:522 prompt | Gemini web (lineage), thread th_098a2683, 2026-01-09; not copied |
| gemini-523-prompt | gemini:523 response → gemini:523 prompt | Gemini web (lineage), thread th_098a2683, 2026-01-09; not copied |
| gemini-524-prompt | gemini:524 response → gemini:524 prompt | Gemini web (lineage), thread th_098a2683, 2026-01-09; not copied |
| gemini-525-prompt | gemini:525 response → gemini:525 prompt | Gemini web (lineage), thread th_098a2683, 2026-01-09; not copied |
| gemini-526-prompt | gemini:526 response → gemini:526 prompt | Gemini web (lineage), thread th_098a2683, 2026-01-09; not copied |
| gemini-527-prompt | gemini:527 response → gemini:527 prompt | Gemini web (lineage), thread th_098a2683, 2026-01-09; not copied |
| gemini-528-prompt | gemini:528 response → gemini:528 prompt | Gemini web (lineage), thread th_098a2683, 2026-01-09; not copied |
| gemini-529-prompt | gemini:529 response → gemini:529 prompt | Gemini web (lineage), thread th_098a2683, 2026-01-09; not copied |
| gemini-530-prompt | gemini:530 response → gemini:530 prompt | Gemini web (lineage), thread th_098a2683, 2026-01-09; not copied |
| gemini-1945-prompt | gemini:1945 response → gemini:1945 prompt | Gemini web (lineage), thread th_09b0d86e, 2026-02-20; not copied |
| gemini-1946-prompt | gemini:1946 response → gemini:1946 prompt | Gemini web (lineage), thread th_09b0d86e, 2026-02-20; not copied |
| gemini-1947-prompt | gemini:1947 response → gemini:1947 prompt | Gemini web (lineage), thread th_09b0d86e, 2026-02-20; not copied |
| gemini-1948-prompt | gemini:1948 response → gemini:1948 prompt | Gemini web (lineage), thread th_09b0d86e, 2026-02-20; not copied |
| gemini-2859-prompt | gemini:2859 response → gemini:2859 prompt | Gemini web (lineage), thread th_09cdad49, 2026-03-23; copied: 3 pasted, 0 lifted into archive notes |
| gemini-2860-prompt | gemini:2860 response → gemini:2860 prompt | Gemini web (lineage), thread th_09cdad49, 2026-03-23; not copied |
| gemini-2861-prompt | gemini:2861 response → gemini:2861 prompt | Gemini web (lineage), thread th_09cdad49, 2026-03-23; not copied |
| gemini-2926-prompt | gemini:2926 response → gemini:2926 prompt | Gemini web (lineage), thread th_0a7cbe74, 2026-03-25; copied: 2 pasted, 0 lifted into archive notes |
| gemini-2927-prompt | gemini:2927 response → gemini:2927 prompt | Gemini web (lineage), thread th_0a7cbe74, 2026-03-25; not copied |
| gemini-2928-prompt | gemini:2928 response → gemini:2928 prompt | Gemini web (lineage), thread th_0a7cbe74, 2026-03-25; not copied |
| gemini-2929-prompt | gemini:2929 response → gemini:2929 prompt | Gemini web (lineage), thread th_0a7cbe74, 2026-03-25; not copied |
| gemini-2930-prompt | gemini:2930 response → gemini:2930 prompt | Gemini web (lineage), thread th_0a7cbe74, 2026-03-25; not copied |
| gemini-29-prompt | gemini:29 response → gemini:29 prompt | Gemini web (lineage), thread th_0a9ba32a, 2025-11-25; not copied |
| gemini-30-prompt | gemini:30 response → gemini:30 prompt | Gemini web (lineage), thread th_0a9ba32a, 2025-11-25; not copied |
| gemini-31-prompt | gemini:31 response → gemini:31 prompt | Gemini web (lineage), thread th_0a9ba32a, 2025-11-25; not copied |
| gemini-32-prompt | gemini:32 response → gemini:32 prompt | Gemini web (lineage), thread th_0a9ba32a, 2025-11-25; not copied |
| gemini-33-prompt | gemini:33 response → gemini:33 prompt | Gemini web (lineage), thread th_0a9ba32a, 2025-11-25; not copied |
| gemini-34-prompt | gemini:34 response → gemini:34 prompt | Gemini web (lineage), thread th_0a9ba32a, 2025-11-25; not copied |
| gemini-35-prompt | gemini:35 response → gemini:35 prompt | Gemini web (lineage), thread th_0a9ba32a, 2025-11-25; not copied |
| gemini-36-prompt | gemini:36 response → gemini:36 prompt | Gemini web (lineage), thread th_0a9ba32a, 2025-11-25; not copied |
| gemini-37-prompt | gemini:37 response → gemini:37 prompt | Gemini web (lineage), thread th_0a9ba32a, 2025-11-25; not copied |
| gemini-1682-prompt | gemini:1682 response → gemini:1682 prompt | Gemini web (lineage), thread th_0b1f354a, 2026-02-10; not copied |
| gemini-1683-prompt | gemini:1683 response → gemini:1683 prompt | Gemini web (lineage), thread th_0b1f354a, 2026-02-10; not copied |
| gemini-1684-prompt | gemini:1684 response → gemini:1684 prompt | Gemini web (lineage), thread th_0b1f354a, 2026-02-10; not copied |
| gemini-1685-prompt | gemini:1685 response → gemini:1685 prompt | Gemini web (lineage), thread th_0b1f354a, 2026-02-10; not copied |
| gemini-1686-prompt | gemini:1686 response → gemini:1686 prompt | Gemini web (lineage), thread th_0b1f354a, 2026-02-10; not copied |
| gemini-1687-prompt | gemini:1687 response → gemini:1687 prompt | Gemini web (lineage), thread th_0b1f354a, 2026-02-10; not copied |
| gemini-1688-prompt | gemini:1688 response → gemini:1688 prompt | Gemini web (lineage), thread th_0b1f354a, 2026-02-10; not copied |
| gemini-1689-prompt | gemini:1689 response → gemini:1689 prompt | Gemini web (lineage), thread th_0b1f354a, 2026-02-10; not copied |
| gemini-1690-prompt | gemini:1690 response → gemini:1690 prompt | Gemini web (lineage), thread th_0b1f354a, 2026-02-10; not copied |
| gemini-2706-prompt | gemini:2706 response → gemini:2706 prompt | Gemini web (lineage), thread th_0b35ac81, 2026-03-16; not copied |
| gemini-2707-prompt | gemini:2707 response → gemini:2707 prompt | Gemini web (lineage), thread th_0b35ac81, 2026-03-16; not copied |
| gemini-2708-prompt | gemini:2708 response → gemini:2708 prompt | Gemini web (lineage), thread th_0b35ac81, 2026-03-16; not copied |
| gemini-321-prompt | gemini:321 response → gemini:321 prompt | Gemini web (lineage), thread th_0bb81222, 2025-12-08; copied: 1 pasted, 0 lifted into archive notes |
| gemini-433-prompt | gemini:433 response → gemini:433 prompt | Gemini web (lineage), thread th_0bdfe982, 2025-12-29; not copied |
| gemini-110-prompt | gemini:110 response → gemini:110 prompt | Gemini web (lineage), thread th_0c10fb7e, 2025-12-02; not copied |
| gemini-111-prompt | gemini:111 response → gemini:111 prompt | Gemini web (lineage), thread th_0c10fb7e, 2025-12-02; copied: 1 pasted, 0 lifted into archive notes |
| gemini-250-prompt | gemini:250 response → gemini:250 prompt | Gemini web (lineage), thread th_0c742832, 2025-12-06; copied: 1 pasted, 0 lifted into archive notes |
| gemini-1874-prompt | gemini:1874 response → gemini:1874 prompt | Gemini web (lineage), thread th_0d2c1d7a, 2026-02-17; not copied |
| gemini-1875-prompt | gemini:1875 response → gemini:1875 prompt | Gemini web (lineage), thread th_0d2c1d7a, 2026-02-17; not copied |
| gemini-1876-prompt | gemini:1876 response → gemini:1876 prompt | Gemini web (lineage), thread th_0d2c1d7a, 2026-02-17; not copied |
| gemini-1877-prompt | gemini:1877 response → gemini:1877 prompt | Gemini web (lineage), thread th_0d2c1d7a, 2026-02-17; not copied |
| gemini-1878-prompt | gemini:1878 response → gemini:1878 prompt | Gemini web (lineage), thread th_0d2c1d7a, 2026-02-17; not copied |
| gemini-1879-prompt | gemini:1879 response → gemini:1879 prompt | Gemini web (lineage), thread th_0d2c1d7a, 2026-02-17; not copied |
| gemini-1880-prompt | gemini:1880 response → gemini:1880 prompt | Gemini web (lineage), thread th_0d2c1d7a, 2026-02-17; not copied |
| gemini-1881-prompt | gemini:1881 response → gemini:1881 prompt | Gemini web (lineage), thread th_0d2c1d7a, 2026-02-17; not copied |
| gemini-1882-prompt | gemini:1882 response → gemini:1882 prompt | Gemini web (lineage), thread th_0d2c1d7a, 2026-02-17; not copied |
| gemini-1883-prompt | gemini:1883 response → gemini:1883 prompt | Gemini web (lineage), thread th_0d2c1d7a, 2026-02-17; not copied |
| gemini-1884-prompt | gemini:1884 response → gemini:1884 prompt | Gemini web (lineage), thread th_0d2c1d7a, 2026-02-17; not copied |
| gemini-1782-prompt | gemini:1782 response → gemini:1782 prompt | Gemini web (lineage), thread th_0dcf7133, 2026-02-13; not copied |
| gemini-1783-prompt | gemini:1783 response → gemini:1783 prompt | Gemini web (lineage), thread th_0dcf7133, 2026-02-13; not copied |
| gemini-84-prompt | gemini:84 response → gemini:84 prompt | Gemini web (lineage), thread th_0e47b418, 2025-12-02; copied: 4 pasted, 0 lifted into archive notes |
| gemini-396-prompt | gemini:396 response → gemini:396 prompt | Gemini web (lineage), thread th_0e97e7d0, 2025-12-28; copied: 1 pasted, 0 lifted into archive notes |
| gemini-397-prompt | gemini:397 response → gemini:397 prompt | Gemini web (lineage), thread th_0e97e7d0, 2025-12-28; not copied |
| gemini-2756-prompt | gemini:2756 response → gemini:2756 prompt | Gemini web (lineage), thread th_0ec14c42, 2026-03-18; not copied |
| gemini-416-prompt | gemini:416 response → gemini:416 prompt | Gemini web (lineage), thread th_0ee80faa, 2025-12-28; not copied |
| gemini-417-prompt | gemini:417 response → gemini:417 prompt | Gemini web (lineage), thread th_0ee80faa, 2025-12-28; not copied |
| gemini-1770-prompt | gemini:1770 response → gemini:1770 prompt | Gemini web (lineage), thread th_0f110906, 2026-02-12; not copied |
| gemini-1771-prompt | gemini:1771 response → gemini:1771 prompt | Gemini web (lineage), thread th_0f110906, 2026-02-12; not copied |
| gemini-2544-prompt | gemini:2544 response → gemini:2544 prompt | Gemini web (lineage), thread th_0f7b0209, 2026-03-08; not copied |
| gemini-2545-prompt | gemini:2545 response → gemini:2545 prompt | Gemini web (lineage), thread th_0f7b0209, 2026-03-08; copied: 2 pasted, 0 lifted into archive notes |
| gemini-806-prompt | gemini:806 response → gemini:806 prompt | Gemini web (lineage), thread th_0f7c9cdc, 2026-01-18; not copied |
| gemini-807-prompt | gemini:807 response → gemini:807 prompt | Gemini web (lineage), thread th_0f7c9cdc, 2026-01-18; not copied |
| gemini-808-prompt | gemini:808 response → gemini:808 prompt | Gemini web (lineage), thread th_0f7c9cdc, 2026-01-18; not copied |
| gemini-809-prompt | gemini:809 response → gemini:809 prompt | Gemini web (lineage), thread th_0f7c9cdc, 2026-01-18; not copied |
| gemini-810-prompt | gemini:810 response → gemini:810 prompt | Gemini web (lineage), thread th_0f7c9cdc, 2026-01-18; not copied |
| gemini-1181-prompt | gemini:1181 response → gemini:1181 prompt | Gemini web (lineage), thread th_0fa88177, 2026-01-27; not copied |
| gemini-1182-prompt | gemini:1182 response → gemini:1182 prompt | Gemini web (lineage), thread th_0fa88177, 2026-01-27; copied: 1 pasted, 0 lifted into archive notes |
| gemini-1183-prompt | gemini:1183 response → gemini:1183 prompt | Gemini web (lineage), thread th_0fa88177, 2026-01-27; not copied |
| gemini-1184-prompt | gemini:1184 response → gemini:1184 prompt | Gemini web (lineage), thread th_0fa88177, 2026-01-27; not copied |
| gemini-1185-prompt | gemini:1185 response → gemini:1185 prompt | Gemini web (lineage), thread th_0fa88177, 2026-01-27; not copied |
| gemini-1186-prompt | gemini:1186 response → gemini:1186 prompt | Gemini web (lineage), thread th_0fa88177, 2026-01-27; copied: 1 pasted, 0 lifted into archive notes |
| gemini-3020-prompt | gemini:3020 response → gemini:3020 prompt | Gemini web (lineage), thread th_0fb3cc85, 2026-03-29; not copied |
| gemini-3021-prompt | gemini:3021 response → gemini:3021 prompt | Gemini web (lineage), thread th_0fb3cc85, 2026-03-29; not copied |
| gemini-82-prompt | gemini:82 response → gemini:82 prompt | Gemini web (lineage), thread th_10201481, 2025-12-02; not copied |
| gemini-306-prompt | gemini:306 response → gemini:306 prompt | Gemini web (lineage), thread th_1053aa20, 2025-12-08; copied: 4 pasted, 1 lifted into archive notes |
| gemini-307-prompt | gemini:307 response → gemini:307 prompt | Gemini web (lineage), thread th_1053aa20, 2025-12-08; not copied |
| gemini-308-prompt | gemini:308 response → gemini:308 prompt | Gemini web (lineage), thread th_1053aa20, 2025-12-08; copied: 6 pasted, 0 lifted into archive notes |
| gemini-309-prompt | gemini:309 response → gemini:309 prompt | Gemini web (lineage), thread th_1053aa20, 2025-12-08; not copied |
| gemini-310-prompt | gemini:310 response → gemini:310 prompt | Gemini web (lineage), thread th_1053aa20, 2025-12-08; copied: 2 pasted, 0 lifted into archive notes |
| gemini-382-prompt | gemini:382 response → gemini:382 prompt | Gemini web (lineage), thread th_10e1361f, 2025-12-28; not copied |
| gemini-383-prompt | gemini:383 response → gemini:383 prompt | Gemini web (lineage), thread th_10e1361f, 2025-12-28; not copied |
| gemini-384-prompt | gemini:384 response → gemini:384 prompt | Gemini web (lineage), thread th_10e1361f, 2025-12-28; not copied |
| gemini-385-prompt | gemini:385 response → gemini:385 prompt | Gemini web (lineage), thread th_10e1361f, 2025-12-28; not copied |
| gemini-386-prompt | gemini:386 response → gemini:386 prompt | Gemini web (lineage), thread th_10e1361f, 2025-12-28; not copied |
| gemini-387-prompt | gemini:387 response → gemini:387 prompt | Gemini web (lineage), thread th_10e1361f, 2025-12-28; not copied |
| gemini-388-prompt | gemini:388 response → gemini:388 prompt | Gemini web (lineage), thread th_10e1361f, 2025-12-28; not copied |
| gemini-389-prompt | gemini:389 response → gemini:389 prompt | Gemini web (lineage), thread th_10e1361f, 2025-12-28; not copied |
| gemini-390-prompt | gemini:390 response → gemini:390 prompt | Gemini web (lineage), thread th_10e1361f, 2025-12-28; not copied |
| gemini-391-prompt | gemini:391 response → gemini:391 prompt | Gemini web (lineage), thread th_10e1361f, 2025-12-28; not copied |
| gemini-392-prompt | gemini:392 response → gemini:392 prompt | Gemini web (lineage), thread th_10e1361f, 2025-12-28; not copied |
| gemini-393-prompt | gemini:393 response → gemini:393 prompt | Gemini web (lineage), thread th_10e1361f, 2025-12-28; not copied |
| gemini-394-prompt | gemini:394 response → gemini:394 prompt | Gemini web (lineage), thread th_10e1361f, 2025-12-28; not copied |
| gemini-395-prompt | gemini:395 response → gemini:395 prompt | Gemini web (lineage), thread th_10e1361f, 2025-12-28; not copied |
| gemini-2978-prompt | gemini:2978 response → gemini:2978 prompt | Gemini web (lineage), thread th_1133f629, 2026-03-26; not copied |
| gemini-2979-prompt | gemini:2979 response → gemini:2979 prompt | Gemini web (lineage), thread th_1133f629, 2026-03-26; not copied |
| gemini-1904-prompt | gemini:1904 response → gemini:1904 prompt | Gemini web (lineage), thread th_118eea62, 2026-02-18 to 2026-02-19; not copied |
| gemini-1905-prompt | gemini:1905 response → gemini:1905 prompt | Gemini web (lineage), thread th_118eea62, 2026-02-18 to 2026-02-19; not copied |
| gemini-1906-prompt | gemini:1906 response → gemini:1906 prompt | Gemini web (lineage), thread th_118eea62, 2026-02-18 to 2026-02-19; not copied |
| gemini-408-prompt | gemini:408 response → gemini:408 prompt | Gemini web (lineage), thread th_1191c372, 2025-12-28; not copied |
| gemini-544-prompt | gemini:544 response → gemini:544 prompt | Gemini web (lineage), thread th_11cb648c, 2026-01-09; not copied |
| gemini-545-prompt | gemini:545 response → gemini:545 prompt | Gemini web (lineage), thread th_11cb648c, 2026-01-09; not copied |
| gemini-546-prompt | gemini:546 response → gemini:546 prompt | Gemini web (lineage), thread th_11cb648c, 2026-01-09; not copied |
| gemini-547-prompt | gemini:547 response → gemini:547 prompt | Gemini web (lineage), thread th_11cb648c, 2026-01-09; not copied |
| gemini-548-prompt | gemini:548 response → gemini:548 prompt | Gemini web (lineage), thread th_11cb648c, 2026-01-09; not copied |
| gemini-549-prompt | gemini:549 response → gemini:549 prompt | Gemini web (lineage), thread th_11cb648c, 2026-01-09; not copied |
| gemini-550-prompt | gemini:550 response → gemini:550 prompt | Gemini web (lineage), thread th_11cb648c, 2026-01-09; not copied |
| gemini-551-prompt | gemini:551 response → gemini:551 prompt | Gemini web (lineage), thread th_11cb648c, 2026-01-09; not copied |
| gemini-552-prompt | gemini:552 response → gemini:552 prompt | Gemini web (lineage), thread th_11cb648c, 2026-01-09; not copied |
| gemini-553-prompt | gemini:553 response → gemini:553 prompt | Gemini web (lineage), thread th_11cb648c, 2026-01-09; not copied |
| gemini-554-prompt | gemini:554 response → gemini:554 prompt | Gemini web (lineage), thread th_11cb648c, 2026-01-09; not copied |
| gemini-555-prompt | gemini:555 response → gemini:555 prompt | Gemini web (lineage), thread th_11cb648c, 2026-01-09; not copied |
| gemini-556-prompt | gemini:556 response → gemini:556 prompt | Gemini web (lineage), thread th_11cb648c, 2026-01-09; not copied |
| gemini-557-prompt | gemini:557 response → gemini:557 prompt | Gemini web (lineage), thread th_11cb648c, 2026-01-09; not copied |
| gemini-558-prompt | gemini:558 response → gemini:558 prompt | Gemini web (lineage), thread th_11cb648c, 2026-01-09; not copied |
| gemini-559-prompt | gemini:559 response → gemini:559 prompt | Gemini web (lineage), thread th_11cb648c, 2026-01-09; not copied |
| gemini-560-prompt | gemini:560 response → gemini:560 prompt | Gemini web (lineage), thread th_11cb648c, 2026-01-09; not copied |
| gemini-561-prompt | gemini:561 response → gemini:561 prompt | Gemini web (lineage), thread th_11cb648c, 2026-01-09; not copied |
| gemini-562-prompt | gemini:562 response → gemini:562 prompt | Gemini web (lineage), thread th_11cb648c, 2026-01-09; not copied |
| gemini-367-prompt | gemini:367 response → gemini:367 prompt | Gemini web (lineage), thread th_11d96dbf, 2025-12-27; not copied |
| gemini-1209-prompt | gemini:1209 response → gemini:1209 prompt | Gemini web (lineage), thread th_11f96ec7, 2026-01-28; not copied |
| gemini-1210-prompt | gemini:1210 response → gemini:1210 prompt | Gemini web (lineage), thread th_11f96ec7, 2026-01-28; not copied |
| gemini-2440-prompt | gemini:2440 response → gemini:2440 prompt | Gemini web (lineage), thread th_1221ccd8, 2026-03-06; not copied |
| gemini-2441-prompt | gemini:2441 response → gemini:2441 prompt | Gemini web (lineage), thread th_1221ccd8, 2026-03-06; not copied |
| gemini-2442-prompt | gemini:2442 response → gemini:2442 prompt | Gemini web (lineage), thread th_1221ccd8, 2026-03-06; not copied |
| gemini-2443-prompt | gemini:2443 response → gemini:2443 prompt | Gemini web (lineage), thread th_1221ccd8, 2026-03-06; not copied |
| gemini-2444-prompt | gemini:2444 response → gemini:2444 prompt | Gemini web (lineage), thread th_1221ccd8, 2026-03-06; not copied |
| gemini-2445-prompt | gemini:2445 response → gemini:2445 prompt | Gemini web (lineage), thread th_1221ccd8, 2026-03-06; copied: 2 pasted, 0 lifted into archive notes |
| gemini-2446-prompt | gemini:2446 response → gemini:2446 prompt | Gemini web (lineage), thread th_1221ccd8, 2026-03-06; not copied |
| gemini-2447-prompt | gemini:2447 response → gemini:2447 prompt | Gemini web (lineage), thread th_1221ccd8, 2026-03-06; not copied |
| gemini-2448-prompt | gemini:2448 response → gemini:2448 prompt | Gemini web (lineage), thread th_1221ccd8, 2026-03-06; not copied |
| gemini-2449-prompt | gemini:2449 response → gemini:2449 prompt | Gemini web (lineage), thread th_1221ccd8, 2026-03-06; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2450-prompt | gemini:2450 response → gemini:2450 prompt | Gemini web (lineage), thread th_1221ccd8, 2026-03-06; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2451-prompt | gemini:2451 response → gemini:2451 prompt | Gemini web (lineage), thread th_1221ccd8, 2026-03-06; not copied |
| gemini-2452-prompt | gemini:2452 response → gemini:2452 prompt | Gemini web (lineage), thread th_1221ccd8, 2026-03-06; not copied |
| gemini-3257-prompt | gemini:3257 response → gemini:3257 prompt | Gemini web (lineage), thread th_12abb54d, 2026-06-03; not copied |
| gemini-3258-prompt | gemini:3258 response → gemini:3258 prompt | Gemini web (lineage), thread th_12abb54d, 2026-06-03; not copied |
| gemini-614-prompt | gemini:614 response → gemini:614 prompt | Gemini web (lineage), thread th_12b7a295, 2026-01-10; not copied |
| gemini-185-prompt | gemini:185 response → gemini:185 prompt | Gemini web (lineage), thread th_12fd8555, 2025-12-04; copied: 4 pasted, 0 lifted into archive notes |
| gemini-2077-prompt | gemini:2077 response → gemini:2077 prompt | Gemini web (lineage), thread th_134b0dc8, 2026-02-24; not copied |
| gemini-2078-prompt | gemini:2078 response → gemini:2078 prompt | Gemini web (lineage), thread th_134b0dc8, 2026-02-24; not copied |
| gemini-2079-prompt | gemini:2079 response → gemini:2079 prompt | Gemini web (lineage), thread th_134b0dc8, 2026-02-24; not copied |
| gemini-1547-prompt | gemini:1547 response → gemini:1547 prompt | Gemini web (lineage), thread th_13ae38d3, 2026-02-09; not copied |
| gemini-1548-prompt | gemini:1548 response → gemini:1548 prompt | Gemini web (lineage), thread th_13ae38d3, 2026-02-09; not copied |
| gemini-407-prompt | gemini:407 response → gemini:407 prompt | Gemini web (lineage), thread th_13cad87a, 2025-12-28; copied: 0 pasted, 1 lifted into archive notes |
| gemini-2516-prompt | gemini:2516 response → gemini:2516 prompt | Gemini web (lineage), thread th_1414a825, 2026-03-07; not copied |
| gemini-2479-prompt | gemini:2479 response → gemini:2479 prompt | Gemini web (lineage), thread th_145732c5, 2026-03-06; copied: 2 pasted, 2 lifted into archive notes |
| gemini-2480-prompt | gemini:2480 response → gemini:2480 prompt | Gemini web (lineage), thread th_145732c5, 2026-03-06; not copied |
| gemini-2481-prompt | gemini:2481 response → gemini:2481 prompt | Gemini web (lineage), thread th_145732c5, 2026-03-06; not copied |
| gemini-2482-prompt | gemini:2482 response → gemini:2482 prompt | Gemini web (lineage), thread th_145732c5, 2026-03-06; not copied |
| gemini-2483-prompt | gemini:2483 response → gemini:2483 prompt | Gemini web (lineage), thread th_145732c5, 2026-03-06; copied: 0 pasted, 1 lifted into archive notes |
| gemini-2118-prompt | gemini:2118 response → gemini:2118 prompt | Gemini web (lineage), thread th_14d8184b, 2026-02-25; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2119-prompt | gemini:2119 response → gemini:2119 prompt | Gemini web (lineage), thread th_14d8184b, 2026-02-25; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2120-prompt | gemini:2120 response → gemini:2120 prompt | Gemini web (lineage), thread th_14d8184b, 2026-02-25; copied: 2 pasted, 0 lifted into archive notes |
| gemini-2121-prompt | gemini:2121 response → gemini:2121 prompt | Gemini web (lineage), thread th_14d8184b, 2026-02-25; copied: 1 pasted, 0 lifted into archive notes |
| gemini-3096-prompt | gemini:3096 response → gemini:3096 prompt | Gemini web (lineage), thread th_14deab76, 2026-04-08; not copied |
| gemini-3097-prompt | gemini:3097 response → gemini:3097 prompt | Gemini web (lineage), thread th_14deab76, 2026-04-08; not copied |
| gemini-3098-prompt | gemini:3098 response → gemini:3098 prompt | Gemini web (lineage), thread th_14deab76, 2026-04-08; not copied |
| gemini-3099-prompt | gemini:3099 response → gemini:3099 prompt | Gemini web (lineage), thread th_14deab76, 2026-04-08; not copied |
| gemini-3100-prompt | gemini:3100 response → gemini:3100 prompt | Gemini web (lineage), thread th_14deab76, 2026-04-08; not copied |
| gemini-1229-prompt | gemini:1229 response → gemini:1229 prompt | Gemini web (lineage), thread th_15399fd1, 2026-01-29; not copied |
| gemini-1230-prompt | gemini:1230 response → gemini:1230 prompt | Gemini web (lineage), thread th_15399fd1, 2026-01-29; not copied |
| gemini-1231-prompt | gemini:1231 response → gemini:1231 prompt | Gemini web (lineage), thread th_15399fd1, 2026-01-29; not copied |
| gemini-1232-prompt | gemini:1232 response → gemini:1232 prompt | Gemini web (lineage), thread th_15399fd1, 2026-01-29; not copied |
| gemini-1233-prompt | gemini:1233 response → gemini:1233 prompt | Gemini web (lineage), thread th_15399fd1, 2026-01-29; not copied |
| gemini-1234-prompt | gemini:1234 response → gemini:1234 prompt | Gemini web (lineage), thread th_15399fd1, 2026-01-29; not copied |
| gemini-1235-prompt | gemini:1235 response → gemini:1235 prompt | Gemini web (lineage), thread th_15399fd1, 2026-01-29; not copied |
| gemini-1236-prompt | gemini:1236 response → gemini:1236 prompt | Gemini web (lineage), thread th_15399fd1, 2026-01-29; not copied |
| gemini-1237-prompt | gemini:1237 response → gemini:1237 prompt | Gemini web (lineage), thread th_15399fd1, 2026-01-29; not copied |
| gemini-1238-prompt | gemini:1238 response → gemini:1238 prompt | Gemini web (lineage), thread th_15399fd1, 2026-01-29; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2377-prompt | gemini:2377 response → gemini:2377 prompt | Gemini web (lineage), thread th_155363a9, 2026-03-04; not copied |
| gemini-1536-prompt | gemini:1536 response → gemini:1536 prompt | Gemini web (lineage), thread th_15593ef4, 2026-02-09; not copied |
| gemini-1537-prompt | gemini:1537 response → gemini:1537 prompt | Gemini web (lineage), thread th_15593ef4, 2026-02-09; not copied |
| gemini-1538-prompt | gemini:1538 response → gemini:1538 prompt | Gemini web (lineage), thread th_15593ef4, 2026-02-09; not copied |
| gemini-1539-prompt | gemini:1539 response → gemini:1539 prompt | Gemini web (lineage), thread th_15593ef4, 2026-02-09; not copied |
| gemini-1540-prompt | gemini:1540 response → gemini:1540 prompt | Gemini web (lineage), thread th_15593ef4, 2026-02-09; not copied |
| gemini-226-prompt | gemini:226 response → gemini:226 prompt | Gemini web (lineage), thread th_15b76767, 2025-12-05; not copied |
| gemini-1432-prompt | gemini:1432 response → gemini:1432 prompt | Gemini web (lineage), thread th_15d60fe2, 2026-02-07; not copied |
| gemini-1433-prompt | gemini:1433 response → gemini:1433 prompt | Gemini web (lineage), thread th_15d60fe2, 2026-02-07; not copied |
| gemini-1434-prompt | gemini:1434 response → gemini:1434 prompt | Gemini web (lineage), thread th_15d60fe2, 2026-02-07; not copied |
| gemini-1435-prompt | gemini:1435 response → gemini:1435 prompt | Gemini web (lineage), thread th_15d60fe2, 2026-02-07; not copied |
| gemini-1436-prompt | gemini:1436 response → gemini:1436 prompt | Gemini web (lineage), thread th_15d60fe2, 2026-02-07; not copied |
| gemini-1437-prompt | gemini:1437 response → gemini:1437 prompt | Gemini web (lineage), thread th_15d60fe2, 2026-02-07; not copied |
| gemini-1438-prompt | gemini:1438 response → gemini:1438 prompt | Gemini web (lineage), thread th_15d60fe2, 2026-02-07; not copied |
| gemini-1439-prompt | gemini:1439 response → gemini:1439 prompt | Gemini web (lineage), thread th_15d60fe2, 2026-02-07; not copied |
| gemini-1440-prompt | gemini:1440 response → gemini:1440 prompt | Gemini web (lineage), thread th_15d60fe2, 2026-02-07; copied: 6 pasted, 0 lifted into archive notes |
| gemini-1441-prompt | gemini:1441 response → gemini:1441 prompt | Gemini web (lineage), thread th_15d60fe2, 2026-02-07; not copied |
| gemini-1442-prompt | gemini:1442 response → gemini:1442 prompt | Gemini web (lineage), thread th_15d60fe2, 2026-02-07; not copied |
| gemini-1443-prompt | gemini:1443 response → gemini:1443 prompt | Gemini web (lineage), thread th_15d60fe2, 2026-02-07; copied: 0 pasted, 2 lifted into archive notes |
| gemini-1444-prompt | gemini:1444 response → gemini:1444 prompt | Gemini web (lineage), thread th_15d60fe2, 2026-02-07; not copied |
| gemini-1445-prompt | gemini:1445 response → gemini:1445 prompt | Gemini web (lineage), thread th_15d60fe2, 2026-02-07; not copied |
| gemini-237-prompt | gemini:237 response → gemini:237 prompt | Gemini web (lineage), thread th_15f9eb3d, 2025-12-06; not copied |
| gemini-240-prompt | gemini:240 response → gemini:240 prompt | Gemini web (lineage), thread th_15f9eb3d, 2025-12-06; not copied |
| gemini-241-prompt | gemini:241 response → gemini:241 prompt | Gemini web (lineage), thread th_15f9eb3d, 2025-12-06; copied: 3 pasted, 0 lifted into archive notes |
| gemini-127-prompt | gemini:127 response → gemini:127 prompt | Gemini web (lineage), thread th_1677977a, 2025-12-03; copied: 1 pasted, 0 lifted into archive notes |
| gemini-128-prompt | gemini:128 response → gemini:128 prompt | Gemini web (lineage), thread th_1677977a, 2025-12-03; not copied |
| gemini-129-prompt | gemini:129 response → gemini:129 prompt | Gemini web (lineage), thread th_1677977a, 2025-12-03; not copied |
| gemini-130-prompt | gemini:130 response → gemini:130 prompt | Gemini web (lineage), thread th_1677977a, 2025-12-03; not copied |
| gemini-2412-prompt | gemini:2412 response → gemini:2412 prompt | Gemini web (lineage), thread th_16d02170, 2026-03-05; not copied |
| gemini-2413-prompt | gemini:2413 response → gemini:2413 prompt | Gemini web (lineage), thread th_16d02170, 2026-03-05; copied: 2 pasted, 0 lifted into archive notes |
| gemini-2414-prompt | gemini:2414 response → gemini:2414 prompt | Gemini web (lineage), thread th_16d02170, 2026-03-05; not copied |
| gemini-2415-prompt | gemini:2415 response → gemini:2415 prompt | Gemini web (lineage), thread th_16d02170, 2026-03-05; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2416-prompt | gemini:2416 response → gemini:2416 prompt | Gemini web (lineage), thread th_16d02170, 2026-03-05; not copied |
| gemini-2417-prompt | gemini:2417 response → gemini:2417 prompt | Gemini web (lineage), thread th_16d02170, 2026-03-05; not copied |
| gemini-2418-prompt | gemini:2418 response → gemini:2418 prompt | Gemini web (lineage), thread th_16d02170, 2026-03-05; not copied |
| gemini-2419-prompt | gemini:2419 response → gemini:2419 prompt | Gemini web (lineage), thread th_16d02170, 2026-03-05; not copied |
| gemini-2786-prompt | gemini:2786 response → gemini:2786 prompt | Gemini web (lineage), thread th_171269dd, 2026-03-21; copied: 0 pasted, 1 lifted into archive notes |
| gemini-3243-prompt | gemini:3243 response → gemini:3243 prompt | Gemini web (lineage), thread th_17321947, 2026-04-21; not copied |
| gemini-3244-prompt | gemini:3244 response → gemini:3244 prompt | Gemini web (lineage), thread th_17321947, 2026-04-21; not copied |
| gemini-3245-prompt | gemini:3245 response → gemini:3245 prompt | Gemini web (lineage), thread th_17321947, 2026-04-21; not copied |
| gemini-3246-prompt | gemini:3246 response → gemini:3246 prompt | Gemini web (lineage), thread th_17321947, 2026-04-21; not copied |
| gemini-3247-prompt | gemini:3247 response → gemini:3247 prompt | Gemini web (lineage), thread th_17321947, 2026-04-21; not copied |
| gemini-3248-prompt | gemini:3248 response → gemini:3248 prompt | Gemini web (lineage), thread th_17321947, 2026-04-21; not copied |
| gemini-814-prompt | gemini:814 response → gemini:814 prompt | Gemini web (lineage), thread th_1785060d, 2026-01-18; not copied |
| gemini-1447-prompt | gemini:1447 response → gemini:1447 prompt | Gemini web (lineage), thread th_17ad12bd, 2026-02-07; not copied |
| gemini-1448-prompt | gemini:1448 response → gemini:1448 prompt | Gemini web (lineage), thread th_17ad12bd, 2026-02-07; copied: 1 pasted, 0 lifted into archive notes |
| gemini-1449-prompt | gemini:1449 response → gemini:1449 prompt | Gemini web (lineage), thread th_17ad12bd, 2026-02-07; not copied |
| gemini-1450-prompt | gemini:1450 response → gemini:1450 prompt | Gemini web (lineage), thread th_17ad12bd, 2026-02-07; not copied |
| gemini-1451-prompt | gemini:1451 response → gemini:1451 prompt | Gemini web (lineage), thread th_17ad12bd, 2026-02-07; not copied |
| gemini-1626-prompt | gemini:1626 response → gemini:1626 prompt | Gemini web (lineage), thread th_18595d2b, 2026-02-10; copied: 0 pasted, 1 lifted into archive notes |
| gemini-1627-prompt | gemini:1627 response → gemini:1627 prompt | Gemini web (lineage), thread th_18595d2b, 2026-02-10; not copied |
| gemini-1628-prompt | gemini:1628 response → gemini:1628 prompt | Gemini web (lineage), thread th_18595d2b, 2026-02-10; not copied |
| gemini-1629-prompt | gemini:1629 response → gemini:1629 prompt | Gemini web (lineage), thread th_18595d2b, 2026-02-10; not copied |
| gemini-1630-prompt | gemini:1630 response → gemini:1630 prompt | Gemini web (lineage), thread th_18595d2b, 2026-02-10; not copied |
| gemini-1631-prompt | gemini:1631 response → gemini:1631 prompt | Gemini web (lineage), thread th_18595d2b, 2026-02-10; not copied |
| gemini-1632-prompt | gemini:1632 response → gemini:1632 prompt | Gemini web (lineage), thread th_18595d2b, 2026-02-10; not copied |
| gemini-1633-prompt | gemini:1633 response → gemini:1633 prompt | Gemini web (lineage), thread th_18595d2b, 2026-02-10; not copied |
| gemini-1634-prompt | gemini:1634 response → gemini:1634 prompt | Gemini web (lineage), thread th_18595d2b, 2026-02-10; not copied |
| gemini-1635-prompt | gemini:1635 response → gemini:1635 prompt | Gemini web (lineage), thread th_18595d2b, 2026-02-10; not copied |
| gemini-1636-prompt | gemini:1636 response → gemini:1636 prompt | Gemini web (lineage), thread th_18595d2b, 2026-02-10; not copied |
| gemini-1637-prompt | gemini:1637 response → gemini:1637 prompt | Gemini web (lineage), thread th_18595d2b, 2026-02-10; not copied |
| gemini-1638-prompt | gemini:1638 response → gemini:1638 prompt | Gemini web (lineage), thread th_18595d2b, 2026-02-10; not copied |
| gemini-1639-prompt | gemini:1639 response → gemini:1639 prompt | Gemini web (lineage), thread th_18595d2b, 2026-02-10; not copied |
| gemini-1640-prompt | gemini:1640 response → gemini:1640 prompt | Gemini web (lineage), thread th_18595d2b, 2026-02-10; not copied |
| gemini-1641-prompt | gemini:1641 response → gemini:1641 prompt | Gemini web (lineage), thread th_18595d2b, 2026-02-10; not copied |
| gemini-1642-prompt | gemini:1642 response → gemini:1642 prompt | Gemini web (lineage), thread th_18595d2b, 2026-02-10; not copied |
| gemini-1643-prompt | gemini:1643 response → gemini:1643 prompt | Gemini web (lineage), thread th_18595d2b, 2026-02-10; not copied |
| gemini-1644-prompt | gemini:1644 response → gemini:1644 prompt | Gemini web (lineage), thread th_18595d2b, 2026-02-10; copied: 1 pasted, 0 lifted into archive notes |
| gemini-1645-prompt | gemini:1645 response → gemini:1645 prompt | Gemini web (lineage), thread th_18595d2b, 2026-02-10; not copied |
| gemini-1646-prompt | gemini:1646 response → gemini:1646 prompt | Gemini web (lineage), thread th_18595d2b, 2026-02-10; not copied |
| gemini-1647-prompt | gemini:1647 response → gemini:1647 prompt | Gemini web (lineage), thread th_18595d2b, 2026-02-10; not copied |
| gemini-1648-prompt | gemini:1648 response → gemini:1648 prompt | Gemini web (lineage), thread th_18595d2b, 2026-02-10; not copied |
| gemini-1649-prompt | gemini:1649 response → gemini:1649 prompt | Gemini web (lineage), thread th_18595d2b, 2026-02-10; not copied |
| gemini-1650-prompt | gemini:1650 response → gemini:1650 prompt | Gemini web (lineage), thread th_18595d2b, 2026-02-10; not copied |
| gemini-2680-prompt | gemini:2680 response → gemini:2680 prompt | Gemini web (lineage), thread th_18e866f6, 2026-03-13; not copied |
| gemini-182-prompt | gemini:182 response → gemini:182 prompt | Gemini web (lineage), thread th_18eeb74c, 2025-12-04; copied: 4 pasted, 0 lifted into archive notes |
| gemini-785-prompt | gemini:785 response → gemini:785 prompt | Gemini web (lineage), thread th_19211440, 2026-01-17; not copied |
| gemini-2932-prompt | gemini:2932 response → gemini:2932 prompt | Gemini web (lineage), thread th_1973b086, 2026-03-25; not copied |
| gemini-2933-prompt | gemini:2933 response → gemini:2933 prompt | Gemini web (lineage), thread th_1973b086, 2026-03-25; not copied |
| gemini-2934-prompt | gemini:2934 response → gemini:2934 prompt | Gemini web (lineage), thread th_1973b086, 2026-03-25; not copied |
| gemini-2935-prompt | gemini:2935 response → gemini:2935 prompt | Gemini web (lineage), thread th_1973b086, 2026-03-25; copied: 0 pasted, 1 lifted into archive notes |
| gemini-2936-prompt | gemini:2936 response → gemini:2936 prompt | Gemini web (lineage), thread th_1973b086, 2026-03-25; copied: 2 pasted, 0 lifted into archive notes |
| gemini-2937-prompt | gemini:2937 response → gemini:2937 prompt | Gemini web (lineage), thread th_1973b086, 2026-03-25; not copied |
| gemini-2938-prompt | gemini:2938 response → gemini:2938 prompt | Gemini web (lineage), thread th_1973b086, 2026-03-25; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2939-prompt | gemini:2939 response → gemini:2939 prompt | Gemini web (lineage), thread th_1973b086, 2026-03-25; not copied |
| gemini-2940-prompt | gemini:2940 response → gemini:2940 prompt | Gemini web (lineage), thread th_1973b086, 2026-03-25; not copied |
| gemini-904-prompt | gemini:904 response → gemini:904 prompt | Gemini web (lineage), thread th_19776a1d, 2026-01-22; not copied |
| gemini-905-prompt | gemini:905 response → gemini:905 prompt | Gemini web (lineage), thread th_19776a1d, 2026-01-22; not copied |
| gemini-1525-prompt | gemini:1525 response → gemini:1525 prompt | Gemini web (lineage), thread th_197a88b3, 2026-02-08; not copied |
| gemini-1526-prompt | gemini:1526 response → gemini:1526 prompt | Gemini web (lineage), thread th_197a88b3, 2026-02-08; not copied |
| gemini-1527-prompt | gemini:1527 response → gemini:1527 prompt | Gemini web (lineage), thread th_197a88b3, 2026-02-08; not copied |
| gemini-1286-prompt | gemini:1286 response → gemini:1286 prompt | Gemini web (lineage), thread th_19f9cecd, 2026-02-01; not copied |
| gemini-1287-prompt | gemini:1287 response → gemini:1287 prompt | Gemini web (lineage), thread th_19f9cecd, 2026-02-01; not copied |
| gemini-1288-prompt | gemini:1288 response → gemini:1288 prompt | Gemini web (lineage), thread th_19f9cecd, 2026-02-01; not copied |
| gemini-1289-prompt | gemini:1289 response → gemini:1289 prompt | Gemini web (lineage), thread th_19f9cecd, 2026-02-01; not copied |
| gemini-1290-prompt | gemini:1290 response → gemini:1290 prompt | Gemini web (lineage), thread th_19f9cecd, 2026-02-01; not copied |
| gemini-2660-prompt | gemini:2660 response → gemini:2660 prompt | Gemini web (lineage), thread th_1cb6a9ea, 2026-03-11; not copied |
| gemini-3162-prompt | gemini:3162 response → gemini:3162 prompt | Gemini web (lineage), thread th_1d1072fd, 2026-04-14; not copied |
| gemini-1854-prompt | gemini:1854 response → gemini:1854 prompt | Gemini web (lineage), thread th_1d676480, 2026-02-15; not copied |
| gemini-2463-prompt | gemini:2463 response → gemini:2463 prompt | Gemini web (lineage), thread th_1da13ba2, 2026-03-06; not copied |
| gemini-2464-prompt | gemini:2464 response → gemini:2464 prompt | Gemini web (lineage), thread th_1da13ba2, 2026-03-06; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2465-prompt | gemini:2465 response → gemini:2465 prompt | Gemini web (lineage), thread th_1da13ba2, 2026-03-06; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2466-prompt | gemini:2466 response → gemini:2466 prompt | Gemini web (lineage), thread th_1da13ba2, 2026-03-06; not copied |
| gemini-2467-prompt | gemini:2467 response → gemini:2467 prompt | Gemini web (lineage), thread th_1da13ba2, 2026-03-06; not copied |
| gemini-2468-prompt | gemini:2468 response → gemini:2468 prompt | Gemini web (lineage), thread th_1da13ba2, 2026-03-06; not copied |
| gemini-672-prompt | gemini:672 response → gemini:672 prompt | Gemini web (lineage), thread th_1dc5157c, 2026-01-12; not copied |
| gemini-282-prompt | gemini:282 response → gemini:282 prompt | Gemini web (lineage), thread th_1e6485b4, 2025-12-08; copied: 5 pasted, 0 lifted into archive notes |
| gemini-2850-prompt | gemini:2850 response → gemini:2850 prompt | Gemini web (lineage), thread th_1e9dda0e, 2026-03-22; not copied |
| gemini-2851-prompt | gemini:2851 response → gemini:2851 prompt | Gemini web (lineage), thread th_1e9dda0e, 2026-03-22; not copied |
| gemini-572-prompt | gemini:572 response → gemini:572 prompt | Gemini web (lineage), thread th_1eb42f02, 2026-01-09; not copied |
| gemini-573-prompt | gemini:573 response → gemini:573 prompt | Gemini web (lineage), thread th_1eb42f02, 2026-01-09; not copied |
| gemini-941-prompt | gemini:941 response → gemini:941 prompt | Gemini web (lineage), thread th_1ed54c16, 2026-01-22; not copied |
| gemini-942-prompt | gemini:942 response → gemini:942 prompt | Gemini web (lineage), thread th_1ed54c16, 2026-01-22; not copied |
| gemini-943-prompt | gemini:943 response → gemini:943 prompt | Gemini web (lineage), thread th_1ed54c16, 2026-01-22; not copied |
| gemini-174-prompt | gemini:174 response → gemini:174 prompt | Gemini web (lineage), thread th_1ef8b3df, 2025-12-04; not copied |
| gemini-175-prompt | gemini:175 response → gemini:175 prompt | Gemini web (lineage), thread th_1ef8b3df, 2025-12-04; copied: 2 pasted, 0 lifted into archive notes |
| gemini-1559-prompt | gemini:1559 response → gemini:1559 prompt | Gemini web (lineage), thread th_1f550295, 2026-02-09; not copied |
| gemini-1560-prompt | gemini:1560 response → gemini:1560 prompt | Gemini web (lineage), thread th_1f550295, 2026-02-09; not copied |
| gemini-1561-prompt | gemini:1561 response → gemini:1561 prompt | Gemini web (lineage), thread th_1f550295, 2026-02-09; not copied |
| gemini-1562-prompt | gemini:1562 response → gemini:1562 prompt | Gemini web (lineage), thread th_1f550295, 2026-02-09; not copied |
| gemini-412-prompt | gemini:412 response → gemini:412 prompt | Gemini web (lineage), thread th_1fa5c98e, 2025-12-28; not copied |
| gemini-743-prompt | gemini:743 response → gemini:743 prompt | Gemini web (lineage), thread th_2111e9b7, 2026-01-14; not copied |
| gemini-106-prompt | gemini:106 response → gemini:106 prompt | Gemini web (lineage), thread th_21e9f305, 2025-12-02; copied: 2 pasted, 0 lifted into archive notes |
| gemini-107-prompt | gemini:107 response → gemini:107 prompt | Gemini web (lineage), thread th_21e9f305, 2025-12-02; not copied |
| gemini-108-prompt | gemini:108 response → gemini:108 prompt | Gemini web (lineage), thread th_21e9f305, 2025-12-02; copied: 2 pasted, 0 lifted into archive notes |
| gemini-1541-prompt | gemini:1541 response → gemini:1541 prompt | Gemini web (lineage), thread th_21f71ffc, 2026-02-09; not copied |
| gemini-2924-prompt | gemini:2924 response → gemini:2924 prompt | Gemini web (lineage), thread th_22958e90, 2026-03-25; not copied |
| gemini-2925-prompt | gemini:2925 response → gemini:2925 prompt | Gemini web (lineage), thread th_22958e90, 2026-03-25; not copied |
| gemini-629-prompt | gemini:629 response → gemini:629 prompt | Gemini web (lineage), thread th_22b7cc33, 2026-01-11; copied: 0 pasted, 1 lifted into archive notes |
| gemini-630-prompt | gemini:630 response → gemini:630 prompt | Gemini web (lineage), thread th_22b7cc33, 2026-01-11; not copied |
| gemini-631-prompt | gemini:631 response → gemini:631 prompt | Gemini web (lineage), thread th_22b7cc33, 2026-01-11; not copied |
| gemini-632-prompt | gemini:632 response → gemini:632 prompt | Gemini web (lineage), thread th_22b7cc33, 2026-01-11; not copied |
| gemini-633-prompt | gemini:633 response → gemini:633 prompt | Gemini web (lineage), thread th_22b7cc33, 2026-01-11; not copied |
| gemini-3125-prompt | gemini:3125 response → gemini:3125 prompt | Gemini web (lineage), thread th_22b7e85f, 2026-04-08; not copied |
| gemini-3126-prompt | gemini:3126 response → gemini:3126 prompt | Gemini web (lineage), thread th_22b7e85f, 2026-04-08; not copied |
| gemini-1698-prompt | gemini:1698 response → gemini:1698 prompt | Gemini web (lineage), thread th_23421663, 2026-02-11; not copied |
| gemini-1104-prompt | gemini:1104 response → gemini:1104 prompt | Gemini web (lineage), thread th_239b2df3, 2026-01-26; not copied |
| gemini-2871-prompt | gemini:2871 response → gemini:2871 prompt | Gemini web (lineage), thread th_23a6a2fe, 2026-03-23; not copied |
| gemini-2872-prompt | gemini:2872 response → gemini:2872 prompt | Gemini web (lineage), thread th_23a6a2fe, 2026-03-23; not copied |
| gemini-112-prompt | gemini:112 response → gemini:112 prompt | Gemini web (lineage), thread th_2402b190, 2025-12-02; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2075-prompt | gemini:2075 response → gemini:2075 prompt | Gemini web (lineage), thread th_2465ee29, 2026-02-22; not copied |
| gemini-1785-prompt | gemini:1785 response → gemini:1785 prompt | Gemini web (lineage), thread th_2466219c, 2026-02-13; not copied |
| gemini-242-prompt | gemini:242 response → gemini:242 prompt | Gemini web (lineage), thread th_24b30942, 2025-12-06; copied: 2 pasted, 0 lifted into archive notes |
| gemini-243-prompt | gemini:243 response → gemini:243 prompt | Gemini web (lineage), thread th_24b30942, 2025-12-06; not copied |
| gemini-244-prompt | gemini:244 response → gemini:244 prompt | Gemini web (lineage), thread th_24b30942, 2025-12-06; not copied |
| gemini-245-prompt | gemini:245 response → gemini:245 prompt | Gemini web (lineage), thread th_24b30942, 2025-12-06; copied: 1 pasted, 0 lifted into archive notes |
| gemini-147-prompt | gemini:147 response → gemini:147 prompt | Gemini web (lineage), thread th_254b83fb, 2025-12-04; copied: 1 pasted, 0 lifted into archive notes |
| gemini-1239-prompt | gemini:1239 response → gemini:1239 prompt | Gemini web (lineage), thread th_25c89488, 2026-01-30; not copied |
| gemini-1240-prompt | gemini:1240 response → gemini:1240 prompt | Gemini web (lineage), thread th_25c89488, 2026-01-30; not copied |
| gemini-1108-prompt | gemini:1108 response → gemini:1108 prompt | Gemini web (lineage), thread th_25fb6dfa, 2026-01-26; copied: 2 pasted, 0 lifted into archive notes |
| gemini-1109-prompt | gemini:1109 response → gemini:1109 prompt | Gemini web (lineage), thread th_25fb6dfa, 2026-01-26; not copied |
| gemini-287-prompt | gemini:287 response → gemini:287 prompt | Gemini web (lineage), thread th_2719557e, 2025-12-08; copied: 1 pasted, 0 lifted into archive notes |
| gemini-288-prompt | gemini:288 response → gemini:288 prompt | Gemini web (lineage), thread th_2719557e, 2025-12-08; copied: 5 pasted, 0 lifted into archive notes |
| gemini-289-prompt | gemini:289 response → gemini:289 prompt | Gemini web (lineage), thread th_2719557e, 2025-12-08; not copied |
| gemini-290-prompt | gemini:290 response → gemini:290 prompt | Gemini web (lineage), thread th_2719557e, 2025-12-08; not copied |
| gemini-291-prompt | gemini:291 response → gemini:291 prompt | Gemini web (lineage), thread th_2719557e, 2025-12-08; not copied |
| gemini-3250-prompt | gemini:3250 response → gemini:3250 prompt | Gemini web (lineage), thread th_27377a98, 2026-04-26; not copied |
| gemini-3251-prompt | gemini:3251 response → gemini:3251 prompt | Gemini web (lineage), thread th_27377a98, 2026-04-26; not copied |
| gemini-3252-prompt | gemini:3252 response → gemini:3252 prompt | Gemini web (lineage), thread th_27377a98, 2026-04-26; not copied |
| gemini-1062-prompt | gemini:1062 response → gemini:1062 prompt | Gemini web (lineage), thread th_27471e30, 2026-01-25; not copied |
| gemini-1063-prompt | gemini:1063 response → gemini:1063 prompt | Gemini web (lineage), thread th_27471e30, 2026-01-25; not copied |
| gemini-1064-prompt | gemini:1064 response → gemini:1064 prompt | Gemini web (lineage), thread th_27471e30, 2026-01-25; not copied |
| gemini-1065-prompt | gemini:1065 response → gemini:1065 prompt | Gemini web (lineage), thread th_27471e30, 2026-01-25; copied: 1 pasted, 1 lifted into archive notes |
| gemini-1066-prompt | gemini:1066 response → gemini:1066 prompt | Gemini web (lineage), thread th_27471e30, 2026-01-25; copied: 0 pasted, 1 lifted into archive notes |
| gemini-1067-prompt | gemini:1067 response → gemini:1067 prompt | Gemini web (lineage), thread th_27471e30, 2026-01-25; not copied |
| gemini-1068-prompt | gemini:1068 response → gemini:1068 prompt | Gemini web (lineage), thread th_27471e30, 2026-01-25; not copied |
| gemini-3105-prompt | gemini:3105 response → gemini:3105 prompt | Gemini web (lineage), thread th_27cd34b6, 2026-04-08; not copied |
| gemini-3106-prompt | gemini:3106 response → gemini:3106 prompt | Gemini web (lineage), thread th_27cd34b6, 2026-04-08; not copied |
| gemini-3107-prompt | gemini:3107 response → gemini:3107 prompt | Gemini web (lineage), thread th_27cd34b6, 2026-04-08; not copied |
| gemini-3108-prompt | gemini:3108 response → gemini:3108 prompt | Gemini web (lineage), thread th_27cd34b6, 2026-04-08; not copied |
| gemini-3109-prompt | gemini:3109 response → gemini:3109 prompt | Gemini web (lineage), thread th_27cd34b6, 2026-04-08; not copied |
| gemini-3110-prompt | gemini:3110 response → gemini:3110 prompt | Gemini web (lineage), thread th_27cd34b6, 2026-04-08; not copied |
| gemini-3111-prompt | gemini:3111 response → gemini:3111 prompt | Gemini web (lineage), thread th_27cd34b6, 2026-04-08; not copied |
| gemini-3112-prompt | gemini:3112 response → gemini:3112 prompt | Gemini web (lineage), thread th_27cd34b6, 2026-04-08; not copied |
| gemini-3113-prompt | gemini:3113 response → gemini:3113 prompt | Gemini web (lineage), thread th_27cd34b6, 2026-04-08; not copied |
| gemini-3114-prompt | gemini:3114 response → gemini:3114 prompt | Gemini web (lineage), thread th_27cd34b6, 2026-04-08; not copied |
| gemini-3115-prompt | gemini:3115 response → gemini:3115 prompt | Gemini web (lineage), thread th_27cd34b6, 2026-04-08; not copied |
| gemini-1662-prompt | gemini:1662 response → gemini:1662 prompt | Gemini web (lineage), thread th_284060d8, 2026-02-10; not copied |
| gemini-1663-prompt | gemini:1663 response → gemini:1663 prompt | Gemini web (lineage), thread th_284060d8, 2026-02-10; not copied |
| gemini-1664-prompt | gemini:1664 response → gemini:1664 prompt | Gemini web (lineage), thread th_284060d8, 2026-02-10; not copied |
| gemini-2787-prompt | gemini:2787 response → gemini:2787 prompt | Gemini web (lineage), thread th_285196ca, 2026-03-21; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2788-prompt | gemini:2788 response → gemini:2788 prompt | Gemini web (lineage), thread th_285196ca, 2026-03-21; not copied |
| gemini-2789-prompt | gemini:2789 response → gemini:2789 prompt | Gemini web (lineage), thread th_285196ca, 2026-03-21; not copied |
| gemini-2790-prompt | gemini:2790 response → gemini:2790 prompt | Gemini web (lineage), thread th_285196ca, 2026-03-21; not copied |
| gemini-2791-prompt | gemini:2791 response → gemini:2791 prompt | Gemini web (lineage), thread th_285196ca, 2026-03-21; copied: 4 pasted, 0 lifted into archive notes |
| gemini-2792-prompt | gemini:2792 response → gemini:2792 prompt | Gemini web (lineage), thread th_285196ca, 2026-03-21; copied: 0 pasted, 1 lifted into archive notes |
| gemini-2793-prompt | gemini:2793 response → gemini:2793 prompt | Gemini web (lineage), thread th_285196ca, 2026-03-21; not copied |
| gemini-2794-prompt | gemini:2794 response → gemini:2794 prompt | Gemini web (lineage), thread th_285196ca, 2026-03-21; not copied |
| gemini-2795-prompt | gemini:2795 response → gemini:2795 prompt | Gemini web (lineage), thread th_285196ca, 2026-03-21; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2796-prompt | gemini:2796 response → gemini:2796 prompt | Gemini web (lineage), thread th_285196ca, 2026-03-21; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2797-prompt | gemini:2797 response → gemini:2797 prompt | Gemini web (lineage), thread th_285196ca, 2026-03-21; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2798-prompt | gemini:2798 response → gemini:2798 prompt | Gemini web (lineage), thread th_285196ca, 2026-03-21; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2799-prompt | gemini:2799 response → gemini:2799 prompt | Gemini web (lineage), thread th_285196ca, 2026-03-21; not copied |
| gemini-2800-prompt | gemini:2800 response → gemini:2800 prompt | Gemini web (lineage), thread th_285196ca, 2026-03-21; not copied |
| gemini-2801-prompt | gemini:2801 response → gemini:2801 prompt | Gemini web (lineage), thread th_285196ca, 2026-03-21; not copied |
| gemini-2802-prompt | gemini:2802 response → gemini:2802 prompt | Gemini web (lineage), thread th_285196ca, 2026-03-21; copied: 3 pasted, 0 lifted into archive notes |
| gemini-744-prompt | gemini:744 response → gemini:744 prompt | Gemini web (lineage), thread th_286c32c4, 2026-01-14; not copied |
| gemini-863-prompt | gemini:863 response → gemini:863 prompt | Gemini web (lineage), thread th_286d3f6d, 2026-01-19; not copied |
| gemini-864-prompt | gemini:864 response → gemini:864 prompt | Gemini web (lineage), thread th_286d3f6d, 2026-01-19; not copied |
| gemini-1095-prompt | gemini:1095 response → gemini:1095 prompt | Gemini web (lineage), thread th_28c7af65, 2026-01-26; not copied |
| gemini-285-prompt | gemini:285 response → gemini:285 prompt | Gemini web (lineage), thread th_2981cebb, 2025-12-08; copied: 4 pasted, 0 lifted into archive notes |
| gemini-286-prompt | gemini:286 response → gemini:286 prompt | Gemini web (lineage), thread th_2981cebb, 2025-12-08; copied: 6 pasted, 1 lifted into archive notes |
| gemini-1214-prompt | gemini:1214 response → gemini:1214 prompt | Gemini web (lineage), thread th_2988726c, 2026-01-28; not copied |
| gemini-1215-prompt | gemini:1215 response → gemini:1215 prompt | Gemini web (lineage), thread th_2988726c, 2026-01-28; not copied |
| gemini-1216-prompt | gemini:1216 response → gemini:1216 prompt | Gemini web (lineage), thread th_2988726c, 2026-01-28; not copied |
| gemini-1217-prompt | gemini:1217 response → gemini:1217 prompt | Gemini web (lineage), thread th_2988726c, 2026-01-28; not copied |
| gemini-1218-prompt | gemini:1218 response → gemini:1218 prompt | Gemini web (lineage), thread th_2988726c, 2026-01-28; not copied |
| gemini-1219-prompt | gemini:1219 response → gemini:1219 prompt | Gemini web (lineage), thread th_2988726c, 2026-01-28; not copied |
| gemini-2815-prompt | gemini:2815 response → gemini:2815 prompt | Gemini web (lineage), thread th_29bd4e6f, 2026-03-22; not copied |
| gemini-1533-prompt | gemini:1533 response → gemini:1533 prompt | Gemini web (lineage), thread th_29c42f84, 2026-02-08; not copied |
| gemini-1534-prompt | gemini:1534 response → gemini:1534 prompt | Gemini web (lineage), thread th_29c42f84, 2026-02-08; not copied |
| gemini-1535-prompt | gemini:1535 response → gemini:1535 prompt | Gemini web (lineage), thread th_29c42f84, 2026-02-08; not copied |
| gemini-3093-prompt | gemini:3093 response → gemini:3093 prompt | Gemini web (lineage), thread th_2a0582d5, 2026-04-07; not copied |
| gemini-3094-prompt | gemini:3094 response → gemini:3094 prompt | Gemini web (lineage), thread th_2a0582d5, 2026-04-07; not copied |
| gemini-1180-prompt | gemini:1180 response → gemini:1180 prompt | Gemini web (lineage), thread th_2a12f9ea, 2026-01-27; copied: 5 pasted, 1 lifted into archive notes |
| gemini-1940-prompt | gemini:1940 response → gemini:1940 prompt | Gemini web (lineage), thread th_2a1d8759, 2026-02-19; not copied |
| gemini-1941-prompt | gemini:1941 response → gemini:1941 prompt | Gemini web (lineage), thread th_2a1d8759, 2026-02-19; not copied |
| gemini-1942-prompt | gemini:1942 response → gemini:1942 prompt | Gemini web (lineage), thread th_2a1d8759, 2026-02-19; not copied |
| gemini-1943-prompt | gemini:1943 response → gemini:1943 prompt | Gemini web (lineage), thread th_2a1d8759, 2026-02-19; not copied |
| gemini-1944-prompt | gemini:1944 response → gemini:1944 prompt | Gemini web (lineage), thread th_2a1d8759, 2026-02-19; not copied |
| gemini-3197-prompt | gemini:3197 response → gemini:3197 prompt | Gemini web (lineage), thread th_2a52ecb4, 2026-04-17; not copied |
| gemini-3198-prompt | gemini:3198 response → gemini:3198 prompt | Gemini web (lineage), thread th_2a52ecb4, 2026-04-17; not copied |
| gemini-3199-prompt | gemini:3199 response → gemini:3199 prompt | Gemini web (lineage), thread th_2a52ecb4, 2026-04-17; not copied |
| gemini-1403-prompt | gemini:1403 response → gemini:1403 prompt | Gemini web (lineage), thread th_2a6eeddd, 2026-02-06; copied: 2 pasted, 0 lifted into archive notes |
| gemini-1404-prompt | gemini:1404 response → gemini:1404 prompt | Gemini web (lineage), thread th_2a6eeddd, 2026-02-06; copied: 0 pasted, 1 lifted into archive notes |
| gemini-1405-prompt | gemini:1405 response → gemini:1405 prompt | Gemini web (lineage), thread th_2a6eeddd, 2026-02-06; copied: 1 pasted, 0 lifted into archive notes |
| gemini-1406-prompt | gemini:1406 response → gemini:1406 prompt | Gemini web (lineage), thread th_2a6eeddd, 2026-02-06; not copied |
| gemini-1407-prompt | gemini:1407 response → gemini:1407 prompt | Gemini web (lineage), thread th_2a6eeddd, 2026-02-06; not copied |
| gemini-1408-prompt | gemini:1408 response → gemini:1408 prompt | Gemini web (lineage), thread th_2a6eeddd, 2026-02-06; not copied |
| gemini-1409-prompt | gemini:1409 response → gemini:1409 prompt | Gemini web (lineage), thread th_2a6eeddd, 2026-02-06; not copied |
| gemini-1518-prompt | gemini:1518 response → gemini:1518 prompt | Gemini web (lineage), thread th_2a92c438, 2026-02-08; not copied |
| gemini-1519-prompt | gemini:1519 response → gemini:1519 prompt | Gemini web (lineage), thread th_2a92c438, 2026-02-08; not copied |
| gemini-1520-prompt | gemini:1520 response → gemini:1520 prompt | Gemini web (lineage), thread th_2a92c438, 2026-02-08; copied: 1 pasted, 0 lifted into archive notes |
| gemini-1521-prompt | gemini:1521 response → gemini:1521 prompt | Gemini web (lineage), thread th_2a92c438, 2026-02-08; not copied |
| gemini-1522-prompt | gemini:1522 response → gemini:1522 prompt | Gemini web (lineage), thread th_2a92c438, 2026-02-08; not copied |
| gemini-1523-prompt | gemini:1523 response → gemini:1523 prompt | Gemini web (lineage), thread th_2a92c438, 2026-02-08; not copied |
| gemini-146-prompt | gemini:146 response → gemini:146 prompt | Gemini web (lineage), thread th_2abf576d, 2025-12-04; copied: 3 pasted, 0 lifted into archive notes |
| gemini-2411-prompt | gemini:2411 response → gemini:2411 prompt | Gemini web (lineage), thread th_2b047214, 2026-03-05; copied: 1 pasted, 1 lifted into archive notes |
| gemini-771-prompt | gemini:771 response → gemini:771 prompt | Gemini web (lineage), thread th_2bb2ebad, 2026-01-17; not copied |
| gemini-772-prompt | gemini:772 response → gemini:772 prompt | Gemini web (lineage), thread th_2bb2ebad, 2026-01-17; not copied |
| gemini-773-prompt | gemini:773 response → gemini:773 prompt | Gemini web (lineage), thread th_2bb2ebad, 2026-01-17; not copied |
| gemini-774-prompt | gemini:774 response → gemini:774 prompt | Gemini web (lineage), thread th_2bb2ebad, 2026-01-17; not copied |
| gemini-775-prompt | gemini:775 response → gemini:775 prompt | Gemini web (lineage), thread th_2bb2ebad, 2026-01-17; not copied |
| gemini-776-prompt | gemini:776 response → gemini:776 prompt | Gemini web (lineage), thread th_2bb2ebad, 2026-01-17; not copied |
| gemini-777-prompt | gemini:777 response → gemini:777 prompt | Gemini web (lineage), thread th_2bb2ebad, 2026-01-17; not copied |
| gemini-778-prompt | gemini:778 response → gemini:778 prompt | Gemini web (lineage), thread th_2bb2ebad, 2026-01-17; not copied |
| gemini-779-prompt | gemini:779 response → gemini:779 prompt | Gemini web (lineage), thread th_2bb2ebad, 2026-01-17; not copied |
| gemini-780-prompt | gemini:780 response → gemini:780 prompt | Gemini web (lineage), thread th_2bb2ebad, 2026-01-17; copied: 1 pasted, 0 lifted into archive notes |
| gemini-781-prompt | gemini:781 response → gemini:781 prompt | Gemini web (lineage), thread th_2bb2ebad, 2026-01-17; not copied |
| gemini-2076-prompt | gemini:2076 response → gemini:2076 prompt | Gemini web (lineage), thread th_2cad23bd, 2026-02-23; not copied |
| gemini-625-prompt | gemini:625 response → gemini:625 prompt | Gemini web (lineage), thread th_2d451c3e, 2026-01-10; not copied |
| gemini-626-prompt | gemini:626 response → gemini:626 prompt | Gemini web (lineage), thread th_2d451c3e, 2026-01-10; not copied |
| gemini-1250-prompt | gemini:1250 response → gemini:1250 prompt | Gemini web (lineage), thread th_2d4d8ed3, 2026-01-31; not copied |
| gemini-1251-prompt | gemini:1251 response → gemini:1251 prompt | Gemini web (lineage), thread th_2d4d8ed3, 2026-01-31; not copied |
| gemini-1252-prompt | gemini:1252 response → gemini:1252 prompt | Gemini web (lineage), thread th_2d4d8ed3, 2026-01-31; not copied |
| gemini-1253-prompt | gemini:1253 response → gemini:1253 prompt | Gemini web (lineage), thread th_2d4d8ed3, 2026-01-31; not copied |
| gemini-1254-prompt | gemini:1254 response → gemini:1254 prompt | Gemini web (lineage), thread th_2d4d8ed3, 2026-01-31; not copied |
| gemini-2149-prompt | gemini:2149 response → gemini:2149 prompt | Gemini web (lineage), thread th_2d8198eb, 2026-02-26; not copied |
| gemini-311-prompt | gemini:311 response → gemini:311 prompt | Gemini web (lineage), thread th_2e3a8bbf, 2025-12-08; copied: 3 pasted, 0 lifted into archive notes |
| gemini-312-prompt | gemini:312 response → gemini:312 prompt | Gemini web (lineage), thread th_2e3a8bbf, 2025-12-08; not copied |
| gemini-313-prompt | gemini:313 response → gemini:313 prompt | Gemini web (lineage), thread th_2e3a8bbf, 2025-12-08; not copied |
| gemini-314-prompt | gemini:314 response → gemini:314 prompt | Gemini web (lineage), thread th_2e3a8bbf, 2025-12-08; copied: 1 pasted, 0 lifted into archive notes |
| gemini-315-prompt | gemini:315 response → gemini:315 prompt | Gemini web (lineage), thread th_2e3a8bbf, 2025-12-08; not copied |
| gemini-317-prompt | gemini:317 response → gemini:317 prompt | Gemini web (lineage), thread th_2e3a8bbf, 2025-12-08; not copied |
| gemini-1976-prompt | gemini:1976 response → gemini:1976 prompt | Gemini web (lineage), thread th_2e45e1e7, 2026-02-20; copied: 1 pasted, 0 lifted into archive notes |
| gemini-1977-prompt | gemini:1977 response → gemini:1977 prompt | Gemini web (lineage), thread th_2e45e1e7, 2026-02-20; not copied |
| gemini-1978-prompt | gemini:1978 response → gemini:1978 prompt | Gemini web (lineage), thread th_2e45e1e7, 2026-02-20; not copied |
| gemini-1979-prompt | gemini:1979 response → gemini:1979 prompt | Gemini web (lineage), thread th_2e45e1e7, 2026-02-20; not copied |
| gemini-1980-prompt | gemini:1980 response → gemini:1980 prompt | Gemini web (lineage), thread th_2e45e1e7, 2026-02-20; not copied |
| gemini-1981-prompt | gemini:1981 response → gemini:1981 prompt | Gemini web (lineage), thread th_2e45e1e7, 2026-02-20; not copied |
| gemini-3051-prompt | gemini:3051 response → gemini:3051 prompt | Gemini web (lineage), thread th_2e5338ee, 2026-04-01; not copied |
| gemini-1797-prompt | gemini:1797 response → gemini:1797 prompt | Gemini web (lineage), thread th_2e739eff, 2026-02-13; not copied |
| gemini-1798-prompt | gemini:1798 response → gemini:1798 prompt | Gemini web (lineage), thread th_2e739eff, 2026-02-13; not copied |
| gemini-1799-prompt | gemini:1799 response → gemini:1799 prompt | Gemini web (lineage), thread th_2e739eff, 2026-02-13; not copied |
| gemini-1800-prompt | gemini:1800 response → gemini:1800 prompt | Gemini web (lineage), thread th_2e739eff, 2026-02-13; not copied |
| gemini-1801-prompt | gemini:1801 response → gemini:1801 prompt | Gemini web (lineage), thread th_2e739eff, 2026-02-13; not copied |
| gemini-2839-prompt | gemini:2839 response → gemini:2839 prompt | Gemini web (lineage), thread th_2ee83cd4, 2026-03-22; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2840-prompt | gemini:2840 response → gemini:2840 prompt | Gemini web (lineage), thread th_2ee83cd4, 2026-03-22; not copied |
| gemini-193-prompt | gemini:193 response → gemini:193 prompt | Gemini web (lineage), thread th_2eee995a, 2025-12-04; copied: 2 pasted, 0 lifted into archive notes |
| gemini-194-prompt | gemini:194 response → gemini:194 prompt | Gemini web (lineage), thread th_2eee995a, 2025-12-04; copied: 2 pasted, 0 lifted into archive notes |
| gemini-195-prompt | gemini:195 response → gemini:195 prompt | Gemini web (lineage), thread th_2eee995a, 2025-12-04; not copied |
| gemini-2273-prompt | gemini:2273 response → gemini:2273 prompt | Gemini web (lineage), thread th_2ef6081d, 2026-03-01; not copied |
| gemini-2274-prompt | gemini:2274 response → gemini:2274 prompt | Gemini web (lineage), thread th_2ef6081d, 2026-03-01; not copied |
| gemini-2275-prompt | gemini:2275 response → gemini:2275 prompt | Gemini web (lineage), thread th_2ef6081d, 2026-03-01; not copied |
| gemini-2276-prompt | gemini:2276 response → gemini:2276 prompt | Gemini web (lineage), thread th_2ef6081d, 2026-03-01; not copied |
| gemini-2277-prompt | gemini:2277 response → gemini:2277 prompt | Gemini web (lineage), thread th_2ef6081d, 2026-03-01; not copied |
| gemini-2278-prompt | gemini:2278 response → gemini:2278 prompt | Gemini web (lineage), thread th_2ef6081d, 2026-03-01; not copied |
| gemini-2279-prompt | gemini:2279 response → gemini:2279 prompt | Gemini web (lineage), thread th_2ef6081d, 2026-03-01; not copied |
| gemini-2280-prompt | gemini:2280 response → gemini:2280 prompt | Gemini web (lineage), thread th_2ef6081d, 2026-03-01; not copied |
| gemini-2281-prompt | gemini:2281 response → gemini:2281 prompt | Gemini web (lineage), thread th_2ef6081d, 2026-03-01; not copied |
| gemini-2282-prompt | gemini:2282 response → gemini:2282 prompt | Gemini web (lineage), thread th_2ef6081d, 2026-03-01; not copied |
| gemini-2283-prompt | gemini:2283 response → gemini:2283 prompt | Gemini web (lineage), thread th_2ef6081d, 2026-03-01; not copied |
| gemini-2284-prompt | gemini:2284 response → gemini:2284 prompt | Gemini web (lineage), thread th_2ef6081d, 2026-03-01; not copied |
| gemini-2285-prompt | gemini:2285 response → gemini:2285 prompt | Gemini web (lineage), thread th_2ef6081d, 2026-03-01; not copied |
| gemini-2286-prompt | gemini:2286 response → gemini:2286 prompt | Gemini web (lineage), thread th_2ef6081d, 2026-03-01; not copied |
| gemini-2287-prompt | gemini:2287 response → gemini:2287 prompt | Gemini web (lineage), thread th_2ef6081d, 2026-03-01; not copied |
| gemini-2288-prompt | gemini:2288 response → gemini:2288 prompt | Gemini web (lineage), thread th_2ef6081d, 2026-03-01; not copied |
| gemini-2289-prompt | gemini:2289 response → gemini:2289 prompt | Gemini web (lineage), thread th_2ef6081d, 2026-03-01; not copied |
| gemini-2290-prompt | gemini:2290 response → gemini:2290 prompt | Gemini web (lineage), thread th_2ef6081d, 2026-03-01; not copied |
| gemini-2291-prompt | gemini:2291 response → gemini:2291 prompt | Gemini web (lineage), thread th_2ef6081d, 2026-03-01; not copied |
| gemini-2292-prompt | gemini:2292 response → gemini:2292 prompt | Gemini web (lineage), thread th_2ef6081d, 2026-03-01; not copied |
| gemini-2293-prompt | gemini:2293 response → gemini:2293 prompt | Gemini web (lineage), thread th_2ef6081d, 2026-03-01; not copied |
| gemini-2294-prompt | gemini:2294 response → gemini:2294 prompt | Gemini web (lineage), thread th_2ef6081d, 2026-03-01; not copied |
| gemini-2295-prompt | gemini:2295 response → gemini:2295 prompt | Gemini web (lineage), thread th_2ef6081d, 2026-03-01; not copied |
| gemini-2296-prompt | gemini:2296 response → gemini:2296 prompt | Gemini web (lineage), thread th_2ef6081d, 2026-03-01; not copied |
| gemini-2297-prompt | gemini:2297 response → gemini:2297 prompt | Gemini web (lineage), thread th_2ef6081d, 2026-03-01; not copied |
| gemini-2298-prompt | gemini:2298 response → gemini:2298 prompt | Gemini web (lineage), thread th_2ef6081d, 2026-03-01; not copied |
| gemini-2299-prompt | gemini:2299 response → gemini:2299 prompt | Gemini web (lineage), thread th_2ef6081d, 2026-03-01; not copied |
| gemini-2300-prompt | gemini:2300 response → gemini:2300 prompt | Gemini web (lineage), thread th_2ef6081d, 2026-03-01; not copied |
| gemini-2301-prompt | gemini:2301 response → gemini:2301 prompt | Gemini web (lineage), thread th_2ef6081d, 2026-03-01; not copied |
| gemini-2302-prompt | gemini:2302 response → gemini:2302 prompt | Gemini web (lineage), thread th_2ef6081d, 2026-03-01; not copied |
| gemini-2303-prompt | gemini:2303 response → gemini:2303 prompt | Gemini web (lineage), thread th_2ef6081d, 2026-03-01; not copied |
| gemini-2304-prompt | gemini:2304 response → gemini:2304 prompt | Gemini web (lineage), thread th_2ef6081d, 2026-03-01; not copied |
| gemini-2305-prompt | gemini:2305 response → gemini:2305 prompt | Gemini web (lineage), thread th_2ef6081d, 2026-03-01; not copied |
| gemini-2306-prompt | gemini:2306 response → gemini:2306 prompt | Gemini web (lineage), thread th_2ef6081d, 2026-03-01; not copied |
| gemini-2307-prompt | gemini:2307 response → gemini:2307 prompt | Gemini web (lineage), thread th_2ef6081d, 2026-03-01; not copied |
| gemini-2308-prompt | gemini:2308 response → gemini:2308 prompt | Gemini web (lineage), thread th_2ef6081d, 2026-03-01; not copied |
| gemini-1091-prompt | gemini:1091 response → gemini:1091 prompt | Gemini web (lineage), thread th_2f08e829, 2026-01-26; not copied |
| gemini-1092-prompt | gemini:1092 response → gemini:1092 prompt | Gemini web (lineage), thread th_2f08e829, 2026-01-26; not copied |
| gemini-2980-prompt | gemini:2980 response → gemini:2980 prompt | Gemini web (lineage), thread th_304df16e, 2026-03-26; not copied |
| gemini-2981-prompt | gemini:2981 response → gemini:2981 prompt | Gemini web (lineage), thread th_304df16e, 2026-03-26; not copied |
| gemini-2982-prompt | gemini:2982 response → gemini:2982 prompt | Gemini web (lineage), thread th_304df16e, 2026-03-26; not copied |
| gemini-2983-prompt | gemini:2983 response → gemini:2983 prompt | Gemini web (lineage), thread th_304df16e, 2026-03-26; not copied |
| gemini-2984-prompt | gemini:2984 response → gemini:2984 prompt | Gemini web (lineage), thread th_304df16e, 2026-03-26; not copied |
| gemini-2985-prompt | gemini:2985 response → gemini:2985 prompt | Gemini web (lineage), thread th_304df16e, 2026-03-26; not copied |
| gemini-2986-prompt | gemini:2986 response → gemini:2986 prompt | Gemini web (lineage), thread th_304df16e, 2026-03-26; not copied |
| gemini-2987-prompt | gemini:2987 response → gemini:2987 prompt | Gemini web (lineage), thread th_304df16e, 2026-03-26; not copied |
| gemini-2988-prompt | gemini:2988 response → gemini:2988 prompt | Gemini web (lineage), thread th_304df16e, 2026-03-26; not copied |
| gemini-2989-prompt | gemini:2989 response → gemini:2989 prompt | Gemini web (lineage), thread th_304df16e, 2026-03-26; not copied |
| gemini-2990-prompt | gemini:2990 response → gemini:2990 prompt | Gemini web (lineage), thread th_304df16e, 2026-03-26; not copied |
| gemini-2991-prompt | gemini:2991 response → gemini:2991 prompt | Gemini web (lineage), thread th_304df16e, 2026-03-26; not copied |
| gemini-2992-prompt | gemini:2992 response → gemini:2992 prompt | Gemini web (lineage), thread th_304df16e, 2026-03-26; not copied |
| gemini-2993-prompt | gemini:2993 response → gemini:2993 prompt | Gemini web (lineage), thread th_304df16e, 2026-03-26; not copied |
| gemini-2994-prompt | gemini:2994 response → gemini:2994 prompt | Gemini web (lineage), thread th_304df16e, 2026-03-26; not copied |
| gemini-2995-prompt | gemini:2995 response → gemini:2995 prompt | Gemini web (lineage), thread th_304df16e, 2026-03-26; not copied |
| gemini-2996-prompt | gemini:2996 response → gemini:2996 prompt | Gemini web (lineage), thread th_304df16e, 2026-03-26; not copied |
| gemini-1410-prompt | gemini:1410 response → gemini:1410 prompt | Gemini web (lineage), thread th_30c64525, 2026-02-06; not copied |
| gemini-1411-prompt | gemini:1411 response → gemini:1411 prompt | Gemini web (lineage), thread th_30c64525, 2026-02-06; not copied |
| gemini-1412-prompt | gemini:1412 response → gemini:1412 prompt | Gemini web (lineage), thread th_30c64525, 2026-02-06; not copied |
| gemini-1413-prompt | gemini:1413 response → gemini:1413 prompt | Gemini web (lineage), thread th_30c64525, 2026-02-06; not copied |
| gemini-1414-prompt | gemini:1414 response → gemini:1414 prompt | Gemini web (lineage), thread th_30c64525, 2026-02-06; copied: 1 pasted, 0 lifted into archive notes |
| gemini-1415-prompt | gemini:1415 response → gemini:1415 prompt | Gemini web (lineage), thread th_30c64525, 2026-02-06; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2867-prompt | gemini:2867 response → gemini:2867 prompt | Gemini web (lineage), thread th_30cb312b, 2026-03-23; not copied |
| gemini-1009-prompt | gemini:1009 response → gemini:1009 prompt | Gemini web (lineage), thread th_30e67718, 2026-01-24; not copied |
| gemini-1010-prompt | gemini:1010 response → gemini:1010 prompt | Gemini web (lineage), thread th_30e67718, 2026-01-24; not copied |
| gemini-1011-prompt | gemini:1011 response → gemini:1011 prompt | Gemini web (lineage), thread th_30e67718, 2026-01-24; not copied |
| gemini-1012-prompt | gemini:1012 response → gemini:1012 prompt | Gemini web (lineage), thread th_30e67718, 2026-01-24; not copied |
| gemini-1013-prompt | gemini:1013 response → gemini:1013 prompt | Gemini web (lineage), thread th_30e67718, 2026-01-24; not copied |
| gemini-1014-prompt | gemini:1014 response → gemini:1014 prompt | Gemini web (lineage), thread th_30e67718, 2026-01-24; not copied |
| gemini-1015-prompt | gemini:1015 response → gemini:1015 prompt | Gemini web (lineage), thread th_30e67718, 2026-01-24; not copied |
| gemini-1016-prompt | gemini:1016 response → gemini:1016 prompt | Gemini web (lineage), thread th_30e67718, 2026-01-24; not copied |
| gemini-1017-prompt | gemini:1017 response → gemini:1017 prompt | Gemini web (lineage), thread th_30e67718, 2026-01-24; not copied |
| gemini-1018-prompt | gemini:1018 response → gemini:1018 prompt | Gemini web (lineage), thread th_30e67718, 2026-01-24; not copied |
| gemini-2556-prompt | gemini:2556 response → gemini:2556 prompt | Gemini web (lineage), thread th_3137da7d, 2026-03-08; copied: 4 pasted, 0 lifted into archive notes |
| gemini-2470-prompt | gemini:2470 response → gemini:2470 prompt | Gemini web (lineage), thread th_3143944b, 2026-03-06; not copied |
| gemini-2471-prompt | gemini:2471 response → gemini:2471 prompt | Gemini web (lineage), thread th_3143944b, 2026-03-06; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2472-prompt | gemini:2472 response → gemini:2472 prompt | Gemini web (lineage), thread th_3143944b, 2026-03-06; not copied |
| gemini-2473-prompt | gemini:2473 response → gemini:2473 prompt | Gemini web (lineage), thread th_3143944b, 2026-03-06; not copied |
| gemini-2474-prompt | gemini:2474 response → gemini:2474 prompt | Gemini web (lineage), thread th_3143944b, 2026-03-06; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2475-prompt | gemini:2475 response → gemini:2475 prompt | Gemini web (lineage), thread th_3143944b, 2026-03-06; not copied |
| gemini-2476-prompt | gemini:2476 response → gemini:2476 prompt | Gemini web (lineage), thread th_3143944b, 2026-03-06; copied: 2 pasted, 0 lifted into archive notes |
| gemini-2477-prompt | gemini:2477 response → gemini:2477 prompt | Gemini web (lineage), thread th_3143944b, 2026-03-06; not copied |
| gemini-2478-prompt | gemini:2478 response → gemini:2478 prompt | Gemini web (lineage), thread th_3143944b, 2026-03-06; not copied |
| gemini-2399-prompt | gemini:2399 response → gemini:2399 prompt | Gemini web (lineage), thread th_3184a90f, 2026-03-05; copied: 4 pasted, 0 lifted into archive notes |
| gemini-1398-prompt | gemini:1398 response → gemini:1398 prompt | Gemini web (lineage), thread th_319a5167, 2026-02-06; copied: 8 pasted, 0 lifted into archive notes |
| gemini-2109-prompt | gemini:2109 response → gemini:2109 prompt | Gemini web (lineage), thread th_31b05d7f, 2026-02-25; copied: 3 pasted, 0 lifted into archive notes |
| gemini-2110-prompt | gemini:2110 response → gemini:2110 prompt | Gemini web (lineage), thread th_31b05d7f, 2026-02-25; copied: 17 pasted, 0 lifted into archive notes |
| gemini-2111-prompt | gemini:2111 response → gemini:2111 prompt | Gemini web (lineage), thread th_31b05d7f, 2026-02-25; not copied |
| gemini-2112-prompt | gemini:2112 response → gemini:2112 prompt | Gemini web (lineage), thread th_31b05d7f, 2026-02-25; not copied |
| gemini-2113-prompt | gemini:2113 response → gemini:2113 prompt | Gemini web (lineage), thread th_31b05d7f, 2026-02-25; not copied |
| gemini-2114-prompt | gemini:2114 response → gemini:2114 prompt | Gemini web (lineage), thread th_31b05d7f, 2026-02-25; not copied |
| gemini-2115-prompt | gemini:2115 response → gemini:2115 prompt | Gemini web (lineage), thread th_31b05d7f, 2026-02-25; copied: 1 pasted, 0 lifted into archive notes |
| gemini-1992-prompt | gemini:1992 response → gemini:1992 prompt | Gemini web (lineage), thread th_324d329b, 2026-02-20; not copied |
| gemini-1993-prompt | gemini:1993 response → gemini:1993 prompt | Gemini web (lineage), thread th_324d329b, 2026-02-20; not copied |
| gemini-1994-prompt | gemini:1994 response → gemini:1994 prompt | Gemini web (lineage), thread th_324d329b, 2026-02-20; not copied |
| gemini-1995-prompt | gemini:1995 response → gemini:1995 prompt | Gemini web (lineage), thread th_324d329b, 2026-02-20; not copied |
| gemini-1996-prompt | gemini:1996 response → gemini:1996 prompt | Gemini web (lineage), thread th_324d329b, 2026-02-20; not copied |
| gemini-1997-prompt | gemini:1997 response → gemini:1997 prompt | Gemini web (lineage), thread th_324d329b, 2026-02-20; not copied |
| gemini-1998-prompt | gemini:1998 response → gemini:1998 prompt | Gemini web (lineage), thread th_324d329b, 2026-02-20; not copied |
| gemini-1999-prompt | gemini:1999 response → gemini:1999 prompt | Gemini web (lineage), thread th_324d329b, 2026-02-20; not copied |
| gemini-2000-prompt | gemini:2000 response → gemini:2000 prompt | Gemini web (lineage), thread th_324d329b, 2026-02-20; not copied |
| gemini-2001-prompt | gemini:2001 response → gemini:2001 prompt | Gemini web (lineage), thread th_324d329b, 2026-02-20; not copied |
| gemini-2002-prompt | gemini:2002 response → gemini:2002 prompt | Gemini web (lineage), thread th_324d329b, 2026-02-20; not copied |
| gemini-2003-prompt | gemini:2003 response → gemini:2003 prompt | Gemini web (lineage), thread th_324d329b, 2026-02-20; not copied |
| gemini-2004-prompt | gemini:2004 response → gemini:2004 prompt | Gemini web (lineage), thread th_324d329b, 2026-02-20; not copied |
| gemini-2005-prompt | gemini:2005 response → gemini:2005 prompt | Gemini web (lineage), thread th_324d329b, 2026-02-20; not copied |
| gemini-2006-prompt | gemini:2006 response → gemini:2006 prompt | Gemini web (lineage), thread th_324d329b, 2026-02-20; not copied |
| gemini-2007-prompt | gemini:2007 response → gemini:2007 prompt | Gemini web (lineage), thread th_324d329b, 2026-02-20; not copied |
| gemini-2008-prompt | gemini:2008 response → gemini:2008 prompt | Gemini web (lineage), thread th_324d329b, 2026-02-20; not copied |
| gemini-2009-prompt | gemini:2009 response → gemini:2009 prompt | Gemini web (lineage), thread th_324d329b, 2026-02-20; not copied |
| gemini-2010-prompt | gemini:2010 response → gemini:2010 prompt | Gemini web (lineage), thread th_324d329b, 2026-02-20; not copied |
| gemini-3231-prompt | gemini:3231 response → gemini:3231 prompt | Gemini web (lineage), thread th_329acfb4, 2026-04-19; not copied |
| gemini-805-prompt | gemini:805 response → gemini:805 prompt | Gemini web (lineage), thread th_339bb26a, 2026-01-18; copied: 2 pasted, 0 lifted into archive notes |
| gemini-1322-prompt | gemini:1322 response → gemini:1322 prompt | Gemini web (lineage), thread th_33a90968, 2026-02-04; not copied |
| gemini-1323-prompt | gemini:1323 response → gemini:1323 prompt | Gemini web (lineage), thread th_33a90968, 2026-02-04; not copied |
| gemini-1324-prompt | gemini:1324 response → gemini:1324 prompt | Gemini web (lineage), thread th_33a90968, 2026-02-04; copied: 0 pasted, 1 lifted into archive notes |
| gemini-1325-prompt | gemini:1325 response → gemini:1325 prompt | Gemini web (lineage), thread th_33a90968, 2026-02-04; not copied |
| gemini-1326-prompt | gemini:1326 response → gemini:1326 prompt | Gemini web (lineage), thread th_33a90968, 2026-02-04; not copied |
| gemini-1327-prompt | gemini:1327 response → gemini:1327 prompt | Gemini web (lineage), thread th_33a90968, 2026-02-04; not copied |
| gemini-1328-prompt | gemini:1328 response → gemini:1328 prompt | Gemini web (lineage), thread th_33a90968, 2026-02-04; not copied |
| gemini-1329-prompt | gemini:1329 response → gemini:1329 prompt | Gemini web (lineage), thread th_33a90968, 2026-02-04; not copied |
| gemini-1330-prompt | gemini:1330 response → gemini:1330 prompt | Gemini web (lineage), thread th_33a90968, 2026-02-04; copied: 1 pasted, 0 lifted into archive notes |
| gemini-1331-prompt | gemini:1331 response → gemini:1331 prompt | Gemini web (lineage), thread th_33a90968, 2026-02-04; copied: 1 pasted, 0 lifted into archive notes |
| gemini-1332-prompt | gemini:1332 response → gemini:1332 prompt | Gemini web (lineage), thread th_33a90968, 2026-02-04; not copied |
| gemini-1333-prompt | gemini:1333 response → gemini:1333 prompt | Gemini web (lineage), thread th_33a90968, 2026-02-04; not copied |
| gemini-1334-prompt | gemini:1334 response → gemini:1334 prompt | Gemini web (lineage), thread th_33a90968, 2026-02-04; not copied |
| gemini-1335-prompt | gemini:1335 response → gemini:1335 prompt | Gemini web (lineage), thread th_33a90968, 2026-02-04; not copied |
| gemini-1336-prompt | gemini:1336 response → gemini:1336 prompt | Gemini web (lineage), thread th_33a90968, 2026-02-04; not copied |
| gemini-1337-prompt | gemini:1337 response → gemini:1337 prompt | Gemini web (lineage), thread th_33a90968, 2026-02-04; not copied |
| gemini-1338-prompt | gemini:1338 response → gemini:1338 prompt | Gemini web (lineage), thread th_33a90968, 2026-02-04; not copied |
| gemini-1339-prompt | gemini:1339 response → gemini:1339 prompt | Gemini web (lineage), thread th_33a90968, 2026-02-04; copied: 0 pasted, 1 lifted into archive notes |
| gemini-1340-prompt | gemini:1340 response → gemini:1340 prompt | Gemini web (lineage), thread th_33a90968, 2026-02-04; copied: 1 pasted, 0 lifted into archive notes |
| gemini-1341-prompt | gemini:1341 response → gemini:1341 prompt | Gemini web (lineage), thread th_33a90968, 2026-02-04; copied: 3 pasted, 0 lifted into archive notes |
| gemini-167-prompt | gemini:167 response → gemini:167 prompt | Gemini web (lineage), thread th_33b4acfe, 2025-12-04; copied: 2 pasted, 0 lifted into archive notes |
| gemini-168-prompt | gemini:168 response → gemini:168 prompt | Gemini web (lineage), thread th_33b4acfe, 2025-12-04; copied: 5 pasted, 0 lifted into archive notes |
| gemini-169-prompt | gemini:169 response → gemini:169 prompt | Gemini web (lineage), thread th_33b4acfe, 2025-12-04; not copied |
| gemini-170-prompt | gemini:170 response → gemini:170 prompt | Gemini web (lineage), thread th_33b4acfe, 2025-12-04; copied: 15 pasted, 0 lifted into archive notes |
| gemini-171-prompt | gemini:171 response → gemini:171 prompt | Gemini web (lineage), thread th_33b4acfe, 2025-12-04; copied: 1 pasted, 0 lifted into archive notes |
| gemini-155-prompt | gemini:155 response → gemini:155 prompt | Gemini web (lineage), thread th_33bfecf7, 2025-12-04; copied: 1 pasted, 1 lifted into archive notes |
| gemini-156-prompt | gemini:156 response → gemini:156 prompt | Gemini web (lineage), thread th_33bfecf7, 2025-12-04; copied: 1 pasted, 0 lifted into archive notes |
| gemini-157-prompt | gemini:157 response → gemini:157 prompt | Gemini web (lineage), thread th_33bfecf7, 2025-12-04; not copied |
| gemini-158-prompt | gemini:158 response → gemini:158 prompt | Gemini web (lineage), thread th_33bfecf7, 2025-12-04; copied: 3 pasted, 0 lifted into archive notes |
| gemini-160-prompt | gemini:160 response → gemini:160 prompt | Gemini web (lineage), thread th_33bfecf7, 2025-12-04; copied: 4 pasted, 1 lifted into archive notes |
| gemini-161-prompt | gemini:161 response → gemini:161 prompt | Gemini web (lineage), thread th_33bfecf7, 2025-12-04; not copied |
| gemini-163-prompt | gemini:163 response → gemini:163 prompt | Gemini web (lineage), thread th_33bfecf7, 2025-12-04; not copied |
| gemini-164-prompt | gemini:164 response → gemini:164 prompt | Gemini web (lineage), thread th_33bfecf7, 2025-12-04; not copied |
| gemini-538-prompt | gemini:538 response → gemini:538 prompt | Gemini web (lineage), thread th_33d8a931, 2026-01-09; not copied |
| gemini-539-prompt | gemini:539 response → gemini:539 prompt | Gemini web (lineage), thread th_33d8a931, 2026-01-09; not copied |
| gemini-540-prompt | gemini:540 response → gemini:540 prompt | Gemini web (lineage), thread th_33d8a931, 2026-01-09; not copied |
| gemini-541-prompt | gemini:541 response → gemini:541 prompt | Gemini web (lineage), thread th_33d8a931, 2026-01-09; not copied |
| gemini-542-prompt | gemini:542 response → gemini:542 prompt | Gemini web (lineage), thread th_33d8a931, 2026-01-09; not copied |
| gemini-543-prompt | gemini:543 response → gemini:543 prompt | Gemini web (lineage), thread th_33d8a931, 2026-01-09; not copied |
| gemini-2577-prompt | gemini:2577 response → gemini:2577 prompt | Gemini web (lineage), thread th_33dbeaea, 2026-03-09; not copied |
| gemini-2578-prompt | gemini:2578 response → gemini:2578 prompt | Gemini web (lineage), thread th_33dbeaea, 2026-03-09; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2579-prompt | gemini:2579 response → gemini:2579 prompt | Gemini web (lineage), thread th_33dbeaea, 2026-03-09; not copied |
| gemini-2580-prompt | gemini:2580 response → gemini:2580 prompt | Gemini web (lineage), thread th_33dbeaea, 2026-03-09; copied: 4 pasted, 0 lifted into archive notes |
| gemini-2581-prompt | gemini:2581 response → gemini:2581 prompt | Gemini web (lineage), thread th_33dbeaea, 2026-03-09; not copied |
| gemini-2582-prompt | gemini:2582 response → gemini:2582 prompt | Gemini web (lineage), thread th_33dbeaea, 2026-03-09; not copied |
| gemini-2583-prompt | gemini:2583 response → gemini:2583 prompt | Gemini web (lineage), thread th_33dbeaea, 2026-03-09; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2584-prompt | gemini:2584 response → gemini:2584 prompt | Gemini web (lineage), thread th_33dbeaea, 2026-03-09; not copied |
| gemini-2585-prompt | gemini:2585 response → gemini:2585 prompt | Gemini web (lineage), thread th_33dbeaea, 2026-03-09; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2586-prompt | gemini:2586 response → gemini:2586 prompt | Gemini web (lineage), thread th_33dbeaea, 2026-03-09; copied: 2 pasted, 0 lifted into archive notes |
| gemini-2587-prompt | gemini:2587 response → gemini:2587 prompt | Gemini web (lineage), thread th_33dbeaea, 2026-03-09; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2588-prompt | gemini:2588 response → gemini:2588 prompt | Gemini web (lineage), thread th_33dbeaea, 2026-03-09; not copied |
| gemini-2589-prompt | gemini:2589 response → gemini:2589 prompt | Gemini web (lineage), thread th_33dbeaea, 2026-03-09; not copied |
| gemini-2590-prompt | gemini:2590 response → gemini:2590 prompt | Gemini web (lineage), thread th_33dbeaea, 2026-03-09; copied: 1 pasted, 0 lifted into archive notes |
| gemini-787-prompt | gemini:787 response → gemini:787 prompt | Gemini web (lineage), thread th_340c917d, 2026-01-17; not copied |
| gemini-2378-prompt | gemini:2378 response → gemini:2378 prompt | Gemini web (lineage), thread th_3430c271, 2026-03-04; copied: 3 pasted, 0 lifted into archive notes |
| gemini-2131-prompt | gemini:2131 response → gemini:2131 prompt | Gemini web (lineage), thread th_344fb14c, 2026-02-25; not copied |
| gemini-2132-prompt | gemini:2132 response → gemini:2132 prompt | Gemini web (lineage), thread th_344fb14c, 2026-02-25; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2133-prompt | gemini:2133 response → gemini:2133 prompt | Gemini web (lineage), thread th_344fb14c, 2026-02-25; copied: 1 pasted, 4 lifted into archive notes |
| gemini-1001-prompt | gemini:1001 response → gemini:1001 prompt | Gemini web (lineage), thread th_34b1dc77, 2026-01-24; not copied |
| gemini-894-prompt | gemini:894 response → gemini:894 prompt | Gemini web (lineage), thread th_34dc9da9, 2026-01-22; not copied |
| gemini-895-prompt | gemini:895 response → gemini:895 prompt | Gemini web (lineage), thread th_34dc9da9, 2026-01-22; not copied |
| gemini-165-prompt | gemini:165 response → gemini:165 prompt | Gemini web (lineage), thread th_35139926, 2025-12-04; not copied |
| gemini-166-prompt | gemini:166 response → gemini:166 prompt | Gemini web (lineage), thread th_35139926, 2025-12-04; not copied |
| gemini-825-prompt | gemini:825 response → gemini:825 prompt | Gemini web (lineage), thread th_35a4b8d2, 2026-01-18; not copied |
| gemini-826-prompt | gemini:826 response → gemini:826 prompt | Gemini web (lineage), thread th_35a4b8d2, 2026-01-18; not copied |
| gemini-827-prompt | gemini:827 response → gemini:827 prompt | Gemini web (lineage), thread th_35a4b8d2, 2026-01-18; not copied |
| gemini-828-prompt | gemini:828 response → gemini:828 prompt | Gemini web (lineage), thread th_35a4b8d2, 2026-01-18; not copied |
| gemini-196-prompt | gemini:196 response → gemini:196 prompt | Gemini web (lineage), thread th_35c3808f, 2025-12-05; copied: 1 pasted, 0 lifted into archive notes |
| gemini-908-prompt | gemini:908 response → gemini:908 prompt | Gemini web (lineage), thread th_35e22ad5, 2026-01-22; not copied |
| gemini-2550-prompt | gemini:2550 response → gemini:2550 prompt | Gemini web (lineage), thread th_366ad820, 2026-03-08; copied: 1 pasted, 0 lifted into archive notes |
| gemini-113-prompt | gemini:113 response → gemini:113 prompt | Gemini web (lineage), thread th_36f0238c, 2025-12-02; copied: 2 pasted, 0 lifted into archive notes |
| gemini-114-prompt | gemini:114 response → gemini:114 prompt | Gemini web (lineage), thread th_36f0238c, 2025-12-02; not copied |
| gemini-115-prompt | gemini:115 response → gemini:115 prompt | Gemini web (lineage), thread th_36f0238c, 2025-12-02; not copied |
| gemini-116-prompt | gemini:116 response → gemini:116 prompt | Gemini web (lineage), thread th_36f0238c, 2025-12-02; not copied |
| gemini-117-prompt | gemini:117 response → gemini:117 prompt | Gemini web (lineage), thread th_36f0238c, 2025-12-02; not copied |
| gemini-118-prompt | gemini:118 response → gemini:118 prompt | Gemini web (lineage), thread th_36f0238c, 2025-12-02; not copied |
| gemini-119-prompt | gemini:119 response → gemini:119 prompt | Gemini web (lineage), thread th_36f0238c, 2025-12-02; not copied |
| gemini-120-prompt | gemini:120 response → gemini:120 prompt | Gemini web (lineage), thread th_36f0238c, 2025-12-02; not copied |
| gemini-297-prompt | gemini:297 response → gemini:297 prompt | Gemini web (lineage), thread th_37734b70, 2025-12-08; copied: 3 pasted, 0 lifted into archive notes |
| gemini-258-prompt | gemini:258 response → gemini:258 prompt | Gemini web (lineage), thread th_37c9cd3c, 2025-12-06; copied: 0 pasted, 1 lifted into archive notes |
| gemini-259-prompt | gemini:259 response → gemini:259 prompt | Gemini web (lineage), thread th_37c9cd3c, 2025-12-06; copied: 1 pasted, 0 lifted into archive notes |
| gemini-260-prompt | gemini:260 response → gemini:260 prompt | Gemini web (lineage), thread th_37c9cd3c, 2025-12-06; copied: 0 pasted, 1 lifted into archive notes |
| gemini-261-prompt | gemini:261 response → gemini:261 prompt | Gemini web (lineage), thread th_37c9cd3c, 2025-12-06; copied: 5 pasted, 0 lifted into archive notes |
| gemini-262-prompt | gemini:262 response → gemini:262 prompt | Gemini web (lineage), thread th_37c9cd3c, 2025-12-06; copied: 0 pasted, 1 lifted into archive notes |
| gemini-263-prompt | gemini:263 response → gemini:263 prompt | Gemini web (lineage), thread th_37c9cd3c, 2025-12-06; not copied |
| gemini-264-prompt | gemini:264 response → gemini:264 prompt | Gemini web (lineage), thread th_37c9cd3c, 2025-12-06; copied: 2 pasted, 0 lifted into archive notes |
| gemini-265-prompt | gemini:265 response → gemini:265 prompt | Gemini web (lineage), thread th_37c9cd3c, 2025-12-06; copied: 3 pasted, 0 lifted into archive notes |
| gemini-266-prompt | gemini:266 response → gemini:266 prompt | Gemini web (lineage), thread th_37c9cd3c, 2025-12-06; not copied |
| gemini-267-prompt | gemini:267 response → gemini:267 prompt | Gemini web (lineage), thread th_37c9cd3c, 2025-12-06; copied: 2 pasted, 1 lifted into archive notes |
| gemini-268-prompt | gemini:268 response → gemini:268 prompt | Gemini web (lineage), thread th_37c9cd3c, 2025-12-06; copied: 5 pasted, 0 lifted into archive notes |
| gemini-269-prompt | gemini:269 response → gemini:269 prompt | Gemini web (lineage), thread th_37c9cd3c, 2025-12-06; copied: 3 pasted, 0 lifted into archive notes |
| gemini-270-prompt | gemini:270 response → gemini:270 prompt | Gemini web (lineage), thread th_37c9cd3c, 2025-12-06; copied: 1 pasted, 0 lifted into archive notes |
| gemini-271-prompt | gemini:271 response → gemini:271 prompt | Gemini web (lineage), thread th_37c9cd3c, 2025-12-06; copied: 2 pasted, 2 lifted into archive notes |
| gemini-272-prompt | gemini:272 response → gemini:272 prompt | Gemini web (lineage), thread th_37c9cd3c, 2025-12-06; copied: 6 pasted, 1 lifted into archive notes |
| gemini-273-prompt | gemini:273 response → gemini:273 prompt | Gemini web (lineage), thread th_37c9cd3c, 2025-12-06; not copied |
| gemini-69-prompt | gemini:69 response → gemini:69 prompt | Gemini web (lineage), thread th_37dbf137, 2025-11-26; not copied |
| gemini-760-prompt | gemini:760 response → gemini:760 prompt | Gemini web (lineage), thread th_38459450, 2026-01-17; not copied |
| gemini-761-prompt | gemini:761 response → gemini:761 prompt | Gemini web (lineage), thread th_38459450, 2026-01-17; not copied |
| gemini-762-prompt | gemini:762 response → gemini:762 prompt | Gemini web (lineage), thread th_38459450, 2026-01-17; not copied |
| gemini-763-prompt | gemini:763 response → gemini:763 prompt | Gemini web (lineage), thread th_38459450, 2026-01-17; not copied |
| gemini-764-prompt | gemini:764 response → gemini:764 prompt | Gemini web (lineage), thread th_38459450, 2026-01-17; not copied |
| gemini-765-prompt | gemini:765 response → gemini:765 prompt | Gemini web (lineage), thread th_38459450, 2026-01-17; not copied |
| gemini-766-prompt | gemini:766 response → gemini:766 prompt | Gemini web (lineage), thread th_38459450, 2026-01-17; not copied |
| gemini-767-prompt | gemini:767 response → gemini:767 prompt | Gemini web (lineage), thread th_38459450, 2026-01-17; not copied |
| gemini-768-prompt | gemini:768 response → gemini:768 prompt | Gemini web (lineage), thread th_38459450, 2026-01-17; not copied |
| gemini-769-prompt | gemini:769 response → gemini:769 prompt | Gemini web (lineage), thread th_38459450, 2026-01-17; not copied |
| gemini-770-prompt | gemini:770 response → gemini:770 prompt | Gemini web (lineage), thread th_38459450, 2026-01-17; not copied |
| gemini-3101-prompt | gemini:3101 response → gemini:3101 prompt | Gemini web (lineage), thread th_38bd49ba, 2026-04-08; not copied |
| gemini-3102-prompt | gemini:3102 response → gemini:3102 prompt | Gemini web (lineage), thread th_38bd49ba, 2026-04-08; not copied |
| gemini-1463-prompt | gemini:1463 response → gemini:1463 prompt | Gemini web (lineage), thread th_38eecbcf, 2026-02-07; not copied |
| gemini-1465-prompt | gemini:1465 response → gemini:1465 prompt | Gemini web (lineage), thread th_38eecbcf, 2026-02-07; not copied |
| gemini-1466-prompt | gemini:1466 response → gemini:1466 prompt | Gemini web (lineage), thread th_38eecbcf, 2026-02-07; not copied |
| gemini-1467-prompt | gemini:1467 response → gemini:1467 prompt | Gemini web (lineage), thread th_38eecbcf, 2026-02-07; not copied |
| gemini-1468-prompt | gemini:1468 response → gemini:1468 prompt | Gemini web (lineage), thread th_38eecbcf, 2026-02-07; not copied |
| gemini-1469-prompt | gemini:1469 response → gemini:1469 prompt | Gemini web (lineage), thread th_38eecbcf, 2026-02-07; not copied |
| gemini-1471-prompt | gemini:1471 response → gemini:1471 prompt | Gemini web (lineage), thread th_38eecbcf, 2026-02-07; not copied |
| gemini-1472-prompt | gemini:1472 response → gemini:1472 prompt | Gemini web (lineage), thread th_38eecbcf, 2026-02-07; not copied |
| gemini-1473-prompt | gemini:1473 response → gemini:1473 prompt | Gemini web (lineage), thread th_38eecbcf, 2026-02-07; not copied |
| gemini-3135-prompt | gemini:3135 response → gemini:3135 prompt | Gemini web (lineage), thread th_3941ff4c, 2026-04-10; not copied |
| gemini-3136-prompt | gemini:3136 response → gemini:3136 prompt | Gemini web (lineage), thread th_3941ff4c, 2026-04-10; not copied |
| gemini-3137-prompt | gemini:3137 response → gemini:3137 prompt | Gemini web (lineage), thread th_3941ff4c, 2026-04-10; not copied |
| gemini-3138-prompt | gemini:3138 response → gemini:3138 prompt | Gemini web (lineage), thread th_3941ff4c, 2026-04-10; not copied |
| gemini-3139-prompt | gemini:3139 response → gemini:3139 prompt | Gemini web (lineage), thread th_3941ff4c, 2026-04-10; not copied |
| gemini-3140-prompt | gemini:3140 response → gemini:3140 prompt | Gemini web (lineage), thread th_3941ff4c, 2026-04-10; not copied |
| gemini-3141-prompt | gemini:3141 response → gemini:3141 prompt | Gemini web (lineage), thread th_3941ff4c, 2026-04-10; not copied |
| gemini-3142-prompt | gemini:3142 response → gemini:3142 prompt | Gemini web (lineage), thread th_3941ff4c, 2026-04-10; not copied |
| gemini-3143-prompt | gemini:3143 response → gemini:3143 prompt | Gemini web (lineage), thread th_3941ff4c, 2026-04-10; not copied |
| gemini-3144-prompt | gemini:3144 response → gemini:3144 prompt | Gemini web (lineage), thread th_3941ff4c, 2026-04-10; not copied |
| gemini-3145-prompt | gemini:3145 response → gemini:3145 prompt | Gemini web (lineage), thread th_3941ff4c, 2026-04-10; not copied |
| gemini-3146-prompt | gemini:3146 response → gemini:3146 prompt | Gemini web (lineage), thread th_3941ff4c, 2026-04-10; not copied |
| gemini-3147-prompt | gemini:3147 response → gemini:3147 prompt | Gemini web (lineage), thread th_3941ff4c, 2026-04-10; not copied |
| gemini-3148-prompt | gemini:3148 response → gemini:3148 prompt | Gemini web (lineage), thread th_3941ff4c, 2026-04-10; not copied |
| gemini-3149-prompt | gemini:3149 response → gemini:3149 prompt | Gemini web (lineage), thread th_3941ff4c, 2026-04-10; not copied |
| gemini-3150-prompt | gemini:3150 response → gemini:3150 prompt | Gemini web (lineage), thread th_3941ff4c, 2026-04-10; not copied |
| gemini-3151-prompt | gemini:3151 response → gemini:3151 prompt | Gemini web (lineage), thread th_3941ff4c, 2026-04-10; not copied |
| gemini-3152-prompt | gemini:3152 response → gemini:3152 prompt | Gemini web (lineage), thread th_3941ff4c, 2026-04-10; not copied |
| gemini-3153-prompt | gemini:3153 response → gemini:3153 prompt | Gemini web (lineage), thread th_3941ff4c, 2026-04-10; not copied |
| gemini-3154-prompt | gemini:3154 response → gemini:3154 prompt | Gemini web (lineage), thread th_3941ff4c, 2026-04-10; not copied |
| gemini-3155-prompt | gemini:3155 response → gemini:3155 prompt | Gemini web (lineage), thread th_3941ff4c, 2026-04-10; not copied |
| gemini-1211-prompt | gemini:1211 response → gemini:1211 prompt | Gemini web (lineage), thread th_394f14a9, 2026-01-28; not copied |
| gemini-1212-prompt | gemini:1212 response → gemini:1212 prompt | Gemini web (lineage), thread th_394f14a9, 2026-01-28; not copied |
| gemini-1213-prompt | gemini:1213 response → gemini:1213 prompt | Gemini web (lineage), thread th_394f14a9, 2026-01-28; not copied |
| gemini-293-prompt | gemini:293 response → gemini:293 prompt | Gemini web (lineage), thread th_3986a8ea, 2025-12-08; copied: 3 pasted, 0 lifted into archive notes |
| gemini-294-prompt | gemini:294 response → gemini:294 prompt | Gemini web (lineage), thread th_3986a8ea, 2025-12-08; copied: 2 pasted, 0 lifted into archive notes |
| gemini-295-prompt | gemini:295 response → gemini:295 prompt | Gemini web (lineage), thread th_3986a8ea, 2025-12-08; copied: 1 pasted, 0 lifted into archive notes |
| gemini-296-prompt | gemini:296 response → gemini:296 prompt | Gemini web (lineage), thread th_3986a8ea, 2025-12-08; copied: 1 pasted, 0 lifted into archive notes |
| gemini-1038-prompt | gemini:1038 response → gemini:1038 prompt | Gemini web (lineage), thread th_39a50d64, 2026-01-25; not copied |
| gemini-189-prompt | gemini:189 response → gemini:189 prompt | Gemini web (lineage), thread th_39d0b229, 2025-12-04; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2215-prompt | gemini:2215 response → gemini:2215 prompt | Gemini web (lineage), thread th_3a299f8c, 2026-02-27; not copied |
| gemini-2216-prompt | gemini:2216 response → gemini:2216 prompt | Gemini web (lineage), thread th_3a299f8c, 2026-02-27; not copied |
| gemini-2217-prompt | gemini:2217 response → gemini:2217 prompt | Gemini web (lineage), thread th_3a299f8c, 2026-02-27; not copied |
| gemini-2218-prompt | gemini:2218 response → gemini:2218 prompt | Gemini web (lineage), thread th_3a299f8c, 2026-02-27; not copied |
| gemini-2219-prompt | gemini:2219 response → gemini:2219 prompt | Gemini web (lineage), thread th_3a299f8c, 2026-02-27; not copied |
| gemini-2220-prompt | gemini:2220 response → gemini:2220 prompt | Gemini web (lineage), thread th_3a299f8c, 2026-02-27; not copied |
| gemini-2221-prompt | gemini:2221 response → gemini:2221 prompt | Gemini web (lineage), thread th_3a299f8c, 2026-02-27; not copied |
| gemini-2222-prompt | gemini:2222 response → gemini:2222 prompt | Gemini web (lineage), thread th_3a299f8c, 2026-02-27; not copied |
| gemini-2661-prompt | gemini:2661 response → gemini:2661 prompt | Gemini web (lineage), thread th_3a68821a, 2026-03-11; not copied |
| gemini-2203-prompt | gemini:2203 response → gemini:2203 prompt | Gemini web (lineage), thread th_3a98ab5c, 2026-02-27; copied: 1 pasted, 0 lifted into archive notes |
| gemini-179-prompt | gemini:179 response → gemini:179 prompt | Gemini web (lineage), thread th_3ac3d15b, 2025-12-04; copied: 1 pasted, 0 lifted into archive notes |
| gemini-180-prompt | gemini:180 response → gemini:180 prompt | Gemini web (lineage), thread th_3ac3d15b, 2025-12-04; copied: 2 pasted, 0 lifted into archive notes |
| gemini-181-prompt | gemini:181 response → gemini:181 prompt | Gemini web (lineage), thread th_3ac3d15b, 2025-12-04; not copied |
| gemini-671-prompt | gemini:671 response → gemini:671 prompt | Gemini web (lineage), thread th_3b275b01, 2026-01-12; copied: 1 pasted, 0 lifted into archive notes |
| gemini-86-prompt | gemini:86 response → gemini:86 prompt | Gemini web (lineage), thread th_3b36bdeb, 2025-12-02; copied: 4 pasted, 0 lifted into archive notes |
| gemini-409-prompt | gemini:409 response → gemini:409 prompt | Gemini web (lineage), thread th_3b3e1b2b, 2025-12-28; copied: 1 pasted, 0 lifted into archive notes |
| gemini-1291-prompt | gemini:1291 response → gemini:1291 prompt | Gemini web (lineage), thread th_3b852b60, 2026-02-01; not copied |
| gemini-1292-prompt | gemini:1292 response → gemini:1292 prompt | Gemini web (lineage), thread th_3b852b60, 2026-02-01; not copied |
| gemini-1293-prompt | gemini:1293 response → gemini:1293 prompt | Gemini web (lineage), thread th_3b852b60, 2026-02-01; not copied |
| gemini-1294-prompt | gemini:1294 response → gemini:1294 prompt | Gemini web (lineage), thread th_3b852b60, 2026-02-01; not copied |
| gemini-1295-prompt | gemini:1295 response → gemini:1295 prompt | Gemini web (lineage), thread th_3b852b60, 2026-02-01; not copied |
| gemini-1296-prompt | gemini:1296 response → gemini:1296 prompt | Gemini web (lineage), thread th_3b852b60, 2026-02-01; not copied |
| gemini-1297-prompt | gemini:1297 response → gemini:1297 prompt | Gemini web (lineage), thread th_3b852b60, 2026-02-01; not copied |
| gemini-1298-prompt | gemini:1298 response → gemini:1298 prompt | Gemini web (lineage), thread th_3b852b60, 2026-02-01; not copied |
| gemini-1299-prompt | gemini:1299 response → gemini:1299 prompt | Gemini web (lineage), thread th_3b852b60, 2026-02-01; not copied |
| gemini-1300-prompt | gemini:1300 response → gemini:1300 prompt | Gemini web (lineage), thread th_3b852b60, 2026-02-01; not copied |
| gemini-1301-prompt | gemini:1301 response → gemini:1301 prompt | Gemini web (lineage), thread th_3b852b60, 2026-02-01; copied: 1 pasted, 0 lifted into archive notes |
| gemini-1302-prompt | gemini:1302 response → gemini:1302 prompt | Gemini web (lineage), thread th_3b852b60, 2026-02-01; not copied |
| gemini-1303-prompt | gemini:1303 response → gemini:1303 prompt | Gemini web (lineage), thread th_3b852b60, 2026-02-01; not copied |
| gemini-1304-prompt | gemini:1304 response → gemini:1304 prompt | Gemini web (lineage), thread th_3b852b60, 2026-02-01; not copied |
| gemini-1982-prompt | gemini:1982 response → gemini:1982 prompt | Gemini web (lineage), thread th_3b9e4c13, 2026-02-20; not copied |
| gemini-1983-prompt | gemini:1983 response → gemini:1983 prompt | Gemini web (lineage), thread th_3b9e4c13, 2026-02-20; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2137-prompt | gemini:2137 response → gemini:2137 prompt | Gemini web (lineage), thread th_3d175c64, 2026-02-25; not copied |
| gemini-2138-prompt | gemini:2138 response → gemini:2138 prompt | Gemini web (lineage), thread th_3d175c64, 2026-02-25; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2139-prompt | gemini:2139 response → gemini:2139 prompt | Gemini web (lineage), thread th_3d175c64, 2026-02-25; copied: 4 pasted, 0 lifted into archive notes |
| gemini-2140-prompt | gemini:2140 response → gemini:2140 prompt | Gemini web (lineage), thread th_3d175c64, 2026-02-25; not copied |
| gemini-2141-prompt | gemini:2141 response → gemini:2141 prompt | Gemini web (lineage), thread th_3d175c64, 2026-02-25; not copied |
| gemini-2142-prompt | gemini:2142 response → gemini:2142 prompt | Gemini web (lineage), thread th_3d175c64, 2026-02-25; not copied |
| gemini-2143-prompt | gemini:2143 response → gemini:2143 prompt | Gemini web (lineage), thread th_3d175c64, 2026-02-25; not copied |
| gemini-377-prompt | gemini:377 response → gemini:377 prompt | Gemini web (lineage), thread th_3d45ec56, 2025-12-28; not copied |
| gemini-378-prompt | gemini:378 response → gemini:378 prompt | Gemini web (lineage), thread th_3d45ec56, 2025-12-28; not copied |
| gemini-1004-prompt | gemini:1004 response → gemini:1004 prompt | Gemini web (lineage), thread th_3d46c6ea, 2026-01-24; not copied |
| gemini-1005-prompt | gemini:1005 response → gemini:1005 prompt | Gemini web (lineage), thread th_3d46c6ea, 2026-01-24; not copied |
| gemini-1006-prompt | gemini:1006 response → gemini:1006 prompt | Gemini web (lineage), thread th_3d46c6ea, 2026-01-24; not copied |
| gemini-1007-prompt | gemini:1007 response → gemini:1007 prompt | Gemini web (lineage), thread th_3d46c6ea, 2026-01-24; not copied |
| gemini-745-prompt | gemini:745 response → gemini:745 prompt | Gemini web (lineage), thread th_3d6ccf29, 2026-01-14; copied: 1 pasted, 0 lifted into archive notes |
| gemini-746-prompt | gemini:746 response → gemini:746 prompt | Gemini web (lineage), thread th_3d6ccf29, 2026-01-14; not copied |
| gemini-2255-prompt | gemini:2255 response → gemini:2255 prompt | Gemini web (lineage), thread th_3e1e791d, 2026-02-28; not copied |
| gemini-2256-prompt | gemini:2256 response → gemini:2256 prompt | Gemini web (lineage), thread th_3e1e791d, 2026-02-28; not copied |
| gemini-2257-prompt | gemini:2257 response → gemini:2257 prompt | Gemini web (lineage), thread th_3e1e791d, 2026-02-28; not copied |
| gemini-2258-prompt | gemini:2258 response → gemini:2258 prompt | Gemini web (lineage), thread th_3e1e791d, 2026-02-28; not copied |
| gemini-76-prompt | gemini:76 response → gemini:76 prompt | Gemini web (lineage), thread th_3e2ccb1c, 2025-12-01; not copied |
| gemini-2498-prompt | gemini:2498 response → gemini:2498 prompt | Gemini web (lineage), thread th_3e3c54c2, 2026-03-06; not copied |
| gemini-2401-prompt | gemini:2401 response → gemini:2401 prompt | Gemini web (lineage), thread th_3eead9ec, 2026-03-05; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2402-prompt | gemini:2402 response → gemini:2402 prompt | Gemini web (lineage), thread th_3eead9ec, 2026-03-05; not copied |
| gemini-2403-prompt | gemini:2403 response → gemini:2403 prompt | Gemini web (lineage), thread th_3eead9ec, 2026-03-05; not copied |
| gemini-2404-prompt | gemini:2404 response → gemini:2404 prompt | Gemini web (lineage), thread th_3eead9ec, 2026-03-05; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2405-prompt | gemini:2405 response → gemini:2405 prompt | Gemini web (lineage), thread th_3eead9ec, 2026-03-05; not copied |
| gemini-2406-prompt | gemini:2406 response → gemini:2406 prompt | Gemini web (lineage), thread th_3eead9ec, 2026-03-05; copied: 1 pasted, 1 lifted into archive notes |
| gemini-2407-prompt | gemini:2407 response → gemini:2407 prompt | Gemini web (lineage), thread th_3eead9ec, 2026-03-05; not copied |
| gemini-2408-prompt | gemini:2408 response → gemini:2408 prompt | Gemini web (lineage), thread th_3eead9ec, 2026-03-05; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2409-prompt | gemini:2409 response → gemini:2409 prompt | Gemini web (lineage), thread th_3eead9ec, 2026-03-05; not copied |
| gemini-2410-prompt | gemini:2410 response → gemini:2410 prompt | Gemini web (lineage), thread th_3eead9ec, 2026-03-05; copied: 2 pasted, 0 lifted into archive notes |
| gemini-1859-prompt | gemini:1859 response → gemini:1859 prompt | Gemini web (lineage), thread th_3f1c18c5, 2026-02-17; not copied |
| gemini-1860-prompt | gemini:1860 response → gemini:1860 prompt | Gemini web (lineage), thread th_3f1c18c5, 2026-02-17; not copied |
| gemini-1861-prompt | gemini:1861 response → gemini:1861 prompt | Gemini web (lineage), thread th_3f1c18c5, 2026-02-17; not copied |
| gemini-1862-prompt | gemini:1862 response → gemini:1862 prompt | Gemini web (lineage), thread th_3f1c18c5, 2026-02-17; not copied |
| gemini-1863-prompt | gemini:1863 response → gemini:1863 prompt | Gemini web (lineage), thread th_3f1c18c5, 2026-02-17; not copied |
| gemini-1864-prompt | gemini:1864 response → gemini:1864 prompt | Gemini web (lineage), thread th_3f1c18c5, 2026-02-17; not copied |
| gemini-1865-prompt | gemini:1865 response → gemini:1865 prompt | Gemini web (lineage), thread th_3f1c18c5, 2026-02-17; not copied |
| gemini-1866-prompt | gemini:1866 response → gemini:1866 prompt | Gemini web (lineage), thread th_3f1c18c5, 2026-02-17; not copied |
| gemini-1867-prompt | gemini:1867 response → gemini:1867 prompt | Gemini web (lineage), thread th_3f1c18c5, 2026-02-17; not copied |
| gemini-1868-prompt | gemini:1868 response → gemini:1868 prompt | Gemini web (lineage), thread th_3f1c18c5, 2026-02-17; not copied |
| gemini-1869-prompt | gemini:1869 response → gemini:1869 prompt | Gemini web (lineage), thread th_3f1c18c5, 2026-02-17; not copied |
| gemini-1870-prompt | gemini:1870 response → gemini:1870 prompt | Gemini web (lineage), thread th_3f1c18c5, 2026-02-17; not copied |
| gemini-1871-prompt | gemini:1871 response → gemini:1871 prompt | Gemini web (lineage), thread th_3f1c18c5, 2026-02-17; not copied |
| gemini-1872-prompt | gemini:1872 response → gemini:1872 prompt | Gemini web (lineage), thread th_3f1c18c5, 2026-02-17; not copied |
| gemini-1873-prompt | gemini:1873 response → gemini:1873 prompt | Gemini web (lineage), thread th_3f1c18c5, 2026-02-17; not copied |
| gemini-2315-prompt | gemini:2315 response → gemini:2315 prompt | Gemini web (lineage), thread th_3f3a6228, 2026-03-03; not copied |
| gemini-2316-prompt | gemini:2316 response → gemini:2316 prompt | Gemini web (lineage), thread th_3f3a6228, 2026-03-03; not copied |
| gemini-2374-prompt | gemini:2374 response → gemini:2374 prompt | Gemini web (lineage), thread th_40182d16, 2026-03-04; not copied |
| gemini-2375-prompt | gemini:2375 response → gemini:2375 prompt | Gemini web (lineage), thread th_40182d16, 2026-03-04; not copied |
| gemini-2376-prompt | gemini:2376 response → gemini:2376 prompt | Gemini web (lineage), thread th_40182d16, 2026-03-04; copied: 1 pasted, 0 lifted into archive notes |
| gemini-1563-prompt | gemini:1563 response → gemini:1563 prompt | Gemini web (lineage), thread th_4028d924, 2026-02-09; not copied |
| gemini-1564-prompt | gemini:1564 response → gemini:1564 prompt | Gemini web (lineage), thread th_4028d924, 2026-02-09; not copied |
| gemini-1565-prompt | gemini:1565 response → gemini:1565 prompt | Gemini web (lineage), thread th_4028d924, 2026-02-09; not copied |
| gemini-1566-prompt | gemini:1566 response → gemini:1566 prompt | Gemini web (lineage), thread th_4028d924, 2026-02-09; copied: 9 pasted, 0 lifted into archive notes |
| gemini-2750-prompt | gemini:2750 response → gemini:2750 prompt | Gemini web (lineage), thread th_40829d08, 2026-03-17; not copied |
| gemini-531-prompt | gemini:531 response → gemini:531 prompt | Gemini web (lineage), thread th_412322ad, 2026-01-09; not copied |
| gemini-532-prompt | gemini:532 response → gemini:532 prompt | Gemini web (lineage), thread th_412322ad, 2026-01-09; not copied |
| gemini-533-prompt | gemini:533 response → gemini:533 prompt | Gemini web (lineage), thread th_412322ad, 2026-01-09; not copied |
| gemini-534-prompt | gemini:534 response → gemini:534 prompt | Gemini web (lineage), thread th_412322ad, 2026-01-09; not copied |
| gemini-535-prompt | gemini:535 response → gemini:535 prompt | Gemini web (lineage), thread th_412322ad, 2026-01-09; not copied |
| gemini-536-prompt | gemini:536 response → gemini:536 prompt | Gemini web (lineage), thread th_412322ad, 2026-01-09; not copied |
| gemini-537-prompt | gemini:537 response → gemini:537 prompt | Gemini web (lineage), thread th_412322ad, 2026-01-09; not copied |
| gemini-2629-prompt | gemini:2629 response → gemini:2629 prompt | Gemini web (lineage), thread th_4163a439, 2026-03-10; not copied |
| gemini-380-prompt | gemini:380 response → gemini:380 prompt | Gemini web (lineage), thread th_416e010f, 2025-12-28; not copied |
| gemini-381-prompt | gemini:381 response → gemini:381 prompt | Gemini web (lineage), thread th_416e010f, 2025-12-28; not copied |
| gemini-2345-prompt | gemini:2345 response → gemini:2345 prompt | Gemini web (lineage), thread th_4189fdf4, 2026-03-03; not copied |
| gemini-796-prompt | gemini:796 response → gemini:796 prompt | Gemini web (lineage), thread th_41c34e69, 2026-01-18; copied: 1 pasted, 0 lifted into archive notes |
| gemini-1111-prompt | gemini:1111 response → gemini:1111 prompt | Gemini web (lineage), thread th_41db0ffc, 2026-01-26; copied: 1 pasted, 0 lifted into archive notes |
| gemini-854-prompt | gemini:854 response → gemini:854 prompt | Gemini web (lineage), thread th_42d1c87b, 2026-01-19; not copied |
| gemini-855-prompt | gemini:855 response → gemini:855 prompt | Gemini web (lineage), thread th_42d1c87b, 2026-01-19; not copied |
| gemini-856-prompt | gemini:856 response → gemini:856 prompt | Gemini web (lineage), thread th_42d1c87b, 2026-01-19; not copied |
| gemini-3022-prompt | gemini:3022 response → gemini:3022 prompt | Gemini web (lineage), thread th_42ffb065, 2026-03-29; not copied |
| gemini-3023-prompt | gemini:3023 response → gemini:3023 prompt | Gemini web (lineage), thread th_42ffb065, 2026-03-29; not copied |
| gemini-21-prompt | gemini:21 response → gemini:21 prompt | Gemini web (lineage), thread th_4338b7af, 2025-11-14; not copied |
| gemini-22-prompt | gemini:22 response → gemini:22 prompt | Gemini web (lineage), thread th_4338b7af, 2025-11-14; not copied |
| gemini-23-prompt | gemini:23 response → gemini:23 prompt | Gemini web (lineage), thread th_4338b7af, 2025-11-14; not copied |
| gemini-24-prompt | gemini:24 response → gemini:24 prompt | Gemini web (lineage), thread th_4338b7af, 2025-11-14; not copied |
| gemini-2101-prompt | gemini:2101 response → gemini:2101 prompt | Gemini web (lineage), thread th_43413d6b, 2026-02-25; not copied |
| gemini-2102-prompt | gemini:2102 response → gemini:2102 prompt | Gemini web (lineage), thread th_43413d6b, 2026-02-25; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2103-prompt | gemini:2103 response → gemini:2103 prompt | Gemini web (lineage), thread th_43413d6b, 2026-02-25; not copied |
| gemini-1096-prompt | gemini:1096 response → gemini:1096 prompt | Gemini web (lineage), thread th_4386df5c, 2026-01-26; copied: 6 pasted, 0 lifted into archive notes |
| gemini-1097-prompt | gemini:1097 response → gemini:1097 prompt | Gemini web (lineage), thread th_4386df5c, 2026-01-26; not copied |
| gemini-1098-prompt | gemini:1098 response → gemini:1098 prompt | Gemini web (lineage), thread th_4386df5c, 2026-01-26; not copied |
| gemini-1099-prompt | gemini:1099 response → gemini:1099 prompt | Gemini web (lineage), thread th_4386df5c, 2026-01-26; not copied |
| gemini-664-prompt | gemini:664 response → gemini:664 prompt | Gemini web (lineage), thread th_43f32e33, 2026-01-12; not copied |
| gemini-665-prompt | gemini:665 response → gemini:665 prompt | Gemini web (lineage), thread th_43f32e33, 2026-01-12; not copied |
| gemini-666-prompt | gemini:666 response → gemini:666 prompt | Gemini web (lineage), thread th_43f32e33, 2026-01-12; not copied |
| gemini-667-prompt | gemini:667 response → gemini:667 prompt | Gemini web (lineage), thread th_43f32e33, 2026-01-12; not copied |
| gemini-668-prompt | gemini:668 response → gemini:668 prompt | Gemini web (lineage), thread th_43f32e33, 2026-01-12; not copied |
| gemini-1020-prompt | gemini:1020 response → gemini:1020 prompt | Gemini web (lineage), thread th_444ea1a6, 2026-01-24; not copied |
| gemini-1021-prompt | gemini:1021 response → gemini:1021 prompt | Gemini web (lineage), thread th_444ea1a6, 2026-01-24; not copied |
| gemini-1022-prompt | gemini:1022 response → gemini:1022 prompt | Gemini web (lineage), thread th_444ea1a6, 2026-01-24; not copied |
| gemini-1023-prompt | gemini:1023 response → gemini:1023 prompt | Gemini web (lineage), thread th_444ea1a6, 2026-01-24; not copied |
| gemini-1024-prompt | gemini:1024 response → gemini:1024 prompt | Gemini web (lineage), thread th_444ea1a6, 2026-01-24; not copied |
| gemini-1025-prompt | gemini:1025 response → gemini:1025 prompt | Gemini web (lineage), thread th_444ea1a6, 2026-01-24; not copied |
| gemini-2144-prompt | gemini:2144 response → gemini:2144 prompt | Gemini web (lineage), thread th_4479d337, 2026-02-25; copied: 3 pasted, 0 lifted into archive notes |
| gemini-2145-prompt | gemini:2145 response → gemini:2145 prompt | Gemini web (lineage), thread th_4479d337, 2026-02-25; copied: 1 pasted, 0 lifted into archive notes |
| gemini-748-prompt | gemini:748 response → gemini:748 prompt | Gemini web (lineage), thread th_4486796d, 2026-01-14; copied: 0 pasted, 1 lifted into archive notes |
| gemini-749-prompt | gemini:749 response → gemini:749 prompt | Gemini web (lineage), thread th_4486796d, 2026-01-14; copied: 0 pasted, 2 lifted into archive notes |
| gemini-750-prompt | gemini:750 response → gemini:750 prompt | Gemini web (lineage), thread th_4486796d, 2026-01-14; not copied |
| gemini-751-prompt | gemini:751 response → gemini:751 prompt | Gemini web (lineage), thread th_4486796d, 2026-01-14; not copied |
| gemini-2687-prompt | gemini:2687 response → gemini:2687 prompt | Gemini web (lineage), thread th_4499895b, 2026-03-13; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2688-prompt | gemini:2688 response → gemini:2688 prompt | Gemini web (lineage), thread th_4499895b, 2026-03-13; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2689-prompt | gemini:2689 response → gemini:2689 prompt | Gemini web (lineage), thread th_4499895b, 2026-03-13; not copied |
| gemini-2690-prompt | gemini:2690 response → gemini:2690 prompt | Gemini web (lineage), thread th_4499895b, 2026-03-13; not copied |
| gemini-1602-prompt | gemini:1602 response → gemini:1602 prompt | Gemini web (lineage), thread th_44e7d831, 2026-02-10; copied: 1 pasted, 0 lifted into archive notes |
| gemini-1603-prompt | gemini:1603 response → gemini:1603 prompt | Gemini web (lineage), thread th_44e7d831, 2026-02-10; not copied |
| gemini-1604-prompt | gemini:1604 response → gemini:1604 prompt | Gemini web (lineage), thread th_44e7d831, 2026-02-10; copied: 2 pasted, 1 lifted into archive notes |
| gemini-1605-prompt | gemini:1605 response → gemini:1605 prompt | Gemini web (lineage), thread th_44e7d831, 2026-02-10; not copied |
| gemini-1606-prompt | gemini:1606 response → gemini:1606 prompt | Gemini web (lineage), thread th_44e7d831, 2026-02-10; not copied |
| gemini-1608-prompt | gemini:1608 response → gemini:1608 prompt | Gemini web (lineage), thread th_44e7d831, 2026-02-10; not copied |
| gemini-1609-prompt | gemini:1609 response → gemini:1609 prompt | Gemini web (lineage), thread th_44e7d831, 2026-02-10; not copied |
| gemini-1610-prompt | gemini:1610 response → gemini:1610 prompt | Gemini web (lineage), thread th_44e7d831, 2026-02-10; not copied |
| gemini-1611-prompt | gemini:1611 response → gemini:1611 prompt | Gemini web (lineage), thread th_44e7d831, 2026-02-10; not copied |
| gemini-1613-prompt | gemini:1613 response → gemini:1613 prompt | Gemini web (lineage), thread th_44e7d831, 2026-02-10; not copied |
| gemini-1614-prompt | gemini:1614 response → gemini:1614 prompt | Gemini web (lineage), thread th_44e7d831, 2026-02-10; not copied |
| gemini-1615-prompt | gemini:1615 response → gemini:1615 prompt | Gemini web (lineage), thread th_44e7d831, 2026-02-10; not copied |
| gemini-1616-prompt | gemini:1616 response → gemini:1616 prompt | Gemini web (lineage), thread th_44e7d831, 2026-02-10; not copied |
| gemini-1617-prompt | gemini:1617 response → gemini:1617 prompt | Gemini web (lineage), thread th_44e7d831, 2026-02-10; not copied |
| gemini-1618-prompt | gemini:1618 response → gemini:1618 prompt | Gemini web (lineage), thread th_44e7d831, 2026-02-10; not copied |
| gemini-1619-prompt | gemini:1619 response → gemini:1619 prompt | Gemini web (lineage), thread th_44e7d831, 2026-02-10; not copied |
| gemini-1620-prompt | gemini:1620 response → gemini:1620 prompt | Gemini web (lineage), thread th_44e7d831, 2026-02-10; not copied |
| gemini-1621-prompt | gemini:1621 response → gemini:1621 prompt | Gemini web (lineage), thread th_44e7d831, 2026-02-10; not copied |
| gemini-1622-prompt | gemini:1622 response → gemini:1622 prompt | Gemini web (lineage), thread th_44e7d831, 2026-02-10; not copied |
| gemini-1623-prompt | gemini:1623 response → gemini:1623 prompt | Gemini web (lineage), thread th_44e7d831, 2026-02-10; not copied |
| gemini-1624-prompt | gemini:1624 response → gemini:1624 prompt | Gemini web (lineage), thread th_44e7d831, 2026-02-10; not copied |
| gemini-1625-prompt | gemini:1625 response → gemini:1625 prompt | Gemini web (lineage), thread th_44e7d831, 2026-02-10; not copied |
| gemini-1549-prompt | gemini:1549 response → gemini:1549 prompt | Gemini web (lineage), thread th_44ed56d0, 2026-02-09; copied: 4 pasted, 0 lifted into archive notes |
| gemini-1550-prompt | gemini:1550 response → gemini:1550 prompt | Gemini web (lineage), thread th_44ed56d0, 2026-02-09; not copied |
| gemini-892-prompt | gemini:892 response → gemini:892 prompt | Gemini web (lineage), thread th_451ce8e8, 2026-01-22; not copied |
| gemini-893-prompt | gemini:893 response → gemini:893 prompt | Gemini web (lineage), thread th_451ce8e8, 2026-01-22; not copied |
| gemini-143-prompt | gemini:143 response → gemini:143 prompt | Gemini web (lineage), thread th_4552f6c0, 2025-12-04; copied: 2 pasted, 0 lifted into archive notes |
| gemini-2822-prompt | gemini:2822 response → gemini:2822 prompt | Gemini web (lineage), thread th_456f23c1, 2026-03-22; not copied |
| gemini-2823-prompt | gemini:2823 response → gemini:2823 prompt | Gemini web (lineage), thread th_456f23c1, 2026-03-22; not copied |
| gemini-2824-prompt | gemini:2824 response → gemini:2824 prompt | Gemini web (lineage), thread th_456f23c1, 2026-03-22; not copied |
| gemini-2825-prompt | gemini:2825 response → gemini:2825 prompt | Gemini web (lineage), thread th_456f23c1, 2026-03-22; not copied |
| gemini-2826-prompt | gemini:2826 response → gemini:2826 prompt | Gemini web (lineage), thread th_456f23c1, 2026-03-22; not copied |
| gemini-2827-prompt | gemini:2827 response → gemini:2827 prompt | Gemini web (lineage), thread th_456f23c1, 2026-03-22; not copied |
| gemini-2828-prompt | gemini:2828 response → gemini:2828 prompt | Gemini web (lineage), thread th_456f23c1, 2026-03-22; not copied |
| gemini-2829-prompt | gemini:2829 response → gemini:2829 prompt | Gemini web (lineage), thread th_456f23c1, 2026-03-22; not copied |
| gemini-2830-prompt | gemini:2830 response → gemini:2830 prompt | Gemini web (lineage), thread th_456f23c1, 2026-03-22; not copied |
| gemini-2831-prompt | gemini:2831 response → gemini:2831 prompt | Gemini web (lineage), thread th_456f23c1, 2026-03-22; copied: 2 pasted, 0 lifted into archive notes |
| gemini-598-prompt | gemini:598 response → gemini:598 prompt | Gemini web (lineage), thread th_464c2fc4, 2026-01-10; not copied |
| gemini-599-prompt | gemini:599 response → gemini:599 prompt | Gemini web (lineage), thread th_464c2fc4, 2026-01-10; not copied |
| gemini-600-prompt | gemini:600 response → gemini:600 prompt | Gemini web (lineage), thread th_464c2fc4, 2026-01-10; not copied |
| gemini-601-prompt | gemini:601 response → gemini:601 prompt | Gemini web (lineage), thread th_464c2fc4, 2026-01-10; copied: 1 pasted, 0 lifted into archive notes |
| gemini-602-prompt | gemini:602 response → gemini:602 prompt | Gemini web (lineage), thread th_464c2fc4, 2026-01-10; not copied |
| gemini-603-prompt | gemini:603 response → gemini:603 prompt | Gemini web (lineage), thread th_464c2fc4, 2026-01-10; not copied |
| gemini-604-prompt | gemini:604 response → gemini:604 prompt | Gemini web (lineage), thread th_464c2fc4, 2026-01-10; not copied |
| gemini-605-prompt | gemini:605 response → gemini:605 prompt | Gemini web (lineage), thread th_464c2fc4, 2026-01-10; not copied |
| gemini-3004-prompt | gemini:3004 response → gemini:3004 prompt | Gemini web (lineage), thread th_4695023f, 2026-03-26; not copied |
| gemini-3005-prompt | gemini:3005 response → gemini:3005 prompt | Gemini web (lineage), thread th_4695023f, 2026-03-26; not copied |
| gemini-3006-prompt | gemini:3006 response → gemini:3006 prompt | Gemini web (lineage), thread th_4695023f, 2026-03-26; copied: 1 pasted, 0 lifted into archive notes |
| gemini-236-prompt | gemini:236 response → gemini:236 prompt | Gemini web (lineage), thread th_469e64e8, 2025-12-05; copied: 4 pasted, 0 lifted into archive notes |
| gemini-637-prompt | gemini:637 response → gemini:637 prompt | Gemini web (lineage), thread th_46f44496, 2026-01-11; not copied |
| gemini-638-prompt | gemini:638 response → gemini:638 prompt | Gemini web (lineage), thread th_46f44496, 2026-01-11; not copied |
| gemini-639-prompt | gemini:639 response → gemini:639 prompt | Gemini web (lineage), thread th_46f44496, 2026-01-11; not copied |
| gemini-640-prompt | gemini:640 response → gemini:640 prompt | Gemini web (lineage), thread th_46f44496, 2026-01-11; not copied |
| gemini-641-prompt | gemini:641 response → gemini:641 prompt | Gemini web (lineage), thread th_46f44496, 2026-01-11; not copied |
| gemini-642-prompt | gemini:642 response → gemini:642 prompt | Gemini web (lineage), thread th_46f44496, 2026-01-11; not copied |
| gemini-643-prompt | gemini:643 response → gemini:643 prompt | Gemini web (lineage), thread th_46f44496, 2026-01-11; not copied |
| gemini-644-prompt | gemini:644 response → gemini:644 prompt | Gemini web (lineage), thread th_46f44496, 2026-01-11; not copied |
| gemini-231-prompt | gemini:231 response → gemini:231 prompt | Gemini web (lineage), thread th_47077d81, 2025-12-05; not copied |
| gemini-232-prompt | gemini:232 response → gemini:232 prompt | Gemini web (lineage), thread th_47077d81, 2025-12-05; not copied |
| gemini-1075-prompt | gemini:1075 response → gemini:1075 prompt | Gemini web (lineage), thread th_4782f47b, 2026-01-25; not copied |
| gemini-1076-prompt | gemini:1076 response → gemini:1076 prompt | Gemini web (lineage), thread th_4782f47b, 2026-01-25; not copied |
| gemini-3082-prompt | gemini:3082 response → gemini:3082 prompt | Gemini web (lineage), thread th_482e046a, 2026-04-01; not copied |
| gemini-3083-prompt | gemini:3083 response → gemini:3083 prompt | Gemini web (lineage), thread th_482e046a, 2026-04-01; not copied |
| gemini-1739-prompt | gemini:1739 response → gemini:1739 prompt | Gemini web (lineage), thread th_486a0292, 2026-02-12; not copied |
| gemini-1740-prompt | gemini:1740 response → gemini:1740 prompt | Gemini web (lineage), thread th_486a0292, 2026-02-12; not copied |
| gemini-1741-prompt | gemini:1741 response → gemini:1741 prompt | Gemini web (lineage), thread th_486a0292, 2026-02-12; not copied |
| gemini-1742-prompt | gemini:1742 response → gemini:1742 prompt | Gemini web (lineage), thread th_486a0292, 2026-02-12; not copied |
| gemini-1743-prompt | gemini:1743 response → gemini:1743 prompt | Gemini web (lineage), thread th_486a0292, 2026-02-12; not copied |
| gemini-1744-prompt | gemini:1744 response → gemini:1744 prompt | Gemini web (lineage), thread th_486a0292, 2026-02-12; not copied |
| gemini-1745-prompt | gemini:1745 response → gemini:1745 prompt | Gemini web (lineage), thread th_486a0292, 2026-02-12; not copied |
| gemini-1746-prompt | gemini:1746 response → gemini:1746 prompt | Gemini web (lineage), thread th_486a0292, 2026-02-12; not copied |
| gemini-1747-prompt | gemini:1747 response → gemini:1747 prompt | Gemini web (lineage), thread th_486a0292, 2026-02-12; not copied |
| gemini-1748-prompt | gemini:1748 response → gemini:1748 prompt | Gemini web (lineage), thread th_486a0292, 2026-02-12; copied: 0 pasted, 1 lifted into archive notes |
| gemini-436-prompt | gemini:436 response → gemini:436 prompt | Gemini web (lineage), thread th_487d459a, 2026-01-05; not copied |
| gemini-437-prompt | gemini:437 response → gemini:437 prompt | Gemini web (lineage), thread th_487d459a, 2026-01-05; not copied |
| gemini-440-prompt | gemini:440 response → gemini:440 prompt | Gemini web (lineage), thread th_487d459a, 2026-01-05; not copied |
| gemini-441-prompt | gemini:441 response → gemini:441 prompt | Gemini web (lineage), thread th_487d459a, 2026-01-05; not copied |
| gemini-442-prompt | gemini:442 response → gemini:442 prompt | Gemini web (lineage), thread th_487d459a, 2026-01-05; not copied |
| gemini-443-prompt | gemini:443 response → gemini:443 prompt | Gemini web (lineage), thread th_487d459a, 2026-01-05; not copied |
| gemini-444-prompt | gemini:444 response → gemini:444 prompt | Gemini web (lineage), thread th_487d459a, 2026-01-05; not copied |
| gemini-445-prompt | gemini:445 response → gemini:445 prompt | Gemini web (lineage), thread th_487d459a, 2026-01-05; not copied |
| gemini-446-prompt | gemini:446 response → gemini:446 prompt | Gemini web (lineage), thread th_487d459a, 2026-01-05; not copied |
| gemini-447-prompt | gemini:447 response → gemini:447 prompt | Gemini web (lineage), thread th_487d459a, 2026-01-05; not copied |
| gemini-448-prompt | gemini:448 response → gemini:448 prompt | Gemini web (lineage), thread th_487d459a, 2026-01-05; not copied |
| gemini-449-prompt | gemini:449 response → gemini:449 prompt | Gemini web (lineage), thread th_487d459a, 2026-01-05; not copied |
| gemini-221-prompt | gemini:221 response → gemini:221 prompt | Gemini web (lineage), thread th_48fa4eab, 2025-12-05; copied: 1 pasted, 0 lifted into archive notes |
| gemini-812-prompt | gemini:812 response → gemini:812 prompt | Gemini web (lineage), thread th_499edff7, 2026-01-18; not copied |
| gemini-813-prompt | gemini:813 response → gemini:813 prompt | Gemini web (lineage), thread th_499edff7, 2026-01-18; copied: 3 pasted, 0 lifted into archive notes |
| gemini-400-prompt | gemini:400 response → gemini:400 prompt | Gemini web (lineage), thread th_49b726c9, 2025-12-28; copied: 0 pasted, 1 lifted into archive notes |
| gemini-319-prompt | gemini:319 response → gemini:319 prompt | Gemini web (lineage), thread th_49bedc59, 2025-12-08; not copied |
| gemini-320-prompt | gemini:320 response → gemini:320 prompt | Gemini web (lineage), thread th_49fd4f5a, 2025-12-08; copied: 7 pasted, 0 lifted into archive notes |
| gemini-2855-prompt | gemini:2855 response → gemini:2855 prompt | Gemini web (lineage), thread th_4a05d000, 2026-03-23; not copied |
| gemini-910-prompt | gemini:910 response → gemini:910 prompt | Gemini web (lineage), thread th_4a4a141f, 2026-01-22; not copied |
| gemini-81-prompt | gemini:81 response → gemini:81 prompt | Gemini web (lineage), thread th_4ac15264, 2025-12-02; copied: 3 pasted, 0 lifted into archive notes |
| gemini-249-prompt | gemini:249 response → gemini:249 prompt | Gemini web (lineage), thread th_4af55576, 2025-12-06; not copied |
| gemini-1307-prompt | gemini:1307 response → gemini:1307 prompt | Gemini web (lineage), thread th_4b6e65bb, 2026-02-02; copied: 3 pasted, 0 lifted into archive notes |
| gemini-2515-prompt | gemini:2515 response → gemini:2515 prompt | Gemini web (lineage), thread th_4c022356, 2026-03-06; not copied |
| gemini-1796-prompt | gemini:1796 response → gemini:1796 prompt | Gemini web (lineage), thread th_4c3d38b5, 2026-02-13; not copied |
| gemini-1102-prompt | gemini:1102 response → gemini:1102 prompt | Gemini web (lineage), thread th_4cbc1b09, 2026-01-26; not copied |
| gemini-1077-prompt | gemini:1077 response → gemini:1077 prompt | Gemini web (lineage), thread th_4cc6127f, 2026-01-26; not copied |
| gemini-1078-prompt | gemini:1078 response → gemini:1078 prompt | Gemini web (lineage), thread th_4cc6127f, 2026-01-26; not copied |
| gemini-1079-prompt | gemini:1079 response → gemini:1079 prompt | Gemini web (lineage), thread th_4cc6127f, 2026-01-26; not copied |
| gemini-1080-prompt | gemini:1080 response → gemini:1080 prompt | Gemini web (lineage), thread th_4cc6127f, 2026-01-26; not copied |
| gemini-1081-prompt | gemini:1081 response → gemini:1081 prompt | Gemini web (lineage), thread th_4cc6127f, 2026-01-26; not copied |
| gemini-1082-prompt | gemini:1082 response → gemini:1082 prompt | Gemini web (lineage), thread th_4cc6127f, 2026-01-26; not copied |
| gemini-1083-prompt | gemini:1083 response → gemini:1083 prompt | Gemini web (lineage), thread th_4cc6127f, 2026-01-26; not copied |
| gemini-1084-prompt | gemini:1084 response → gemini:1084 prompt | Gemini web (lineage), thread th_4cc6127f, 2026-01-26; not copied |
| gemini-1085-prompt | gemini:1085 response → gemini:1085 prompt | Gemini web (lineage), thread th_4cc6127f, 2026-01-26; not copied |
| gemini-1086-prompt | gemini:1086 response → gemini:1086 prompt | Gemini web (lineage), thread th_4cc6127f, 2026-01-26; not copied |
| gemini-1087-prompt | gemini:1087 response → gemini:1087 prompt | Gemini web (lineage), thread th_4cc6127f, 2026-01-26; not copied |
| gemini-1088-prompt | gemini:1088 response → gemini:1088 prompt | Gemini web (lineage), thread th_4cc6127f, 2026-01-26; not copied |
| gemini-2519-prompt | gemini:2519 response → gemini:2519 prompt | Gemini web (lineage), thread th_4cdc8aee, 2026-03-08; not copied |
| gemini-2520-prompt | gemini:2520 response → gemini:2520 prompt | Gemini web (lineage), thread th_4cdc8aee, 2026-03-08; not copied |
| gemini-2757-prompt | gemini:2757 response → gemini:2757 prompt | Gemini web (lineage), thread th_4d9af7c5, 2026-03-19; not copied |
| gemini-87-prompt | gemini:87 response → gemini:87 prompt | Gemini web (lineage), thread th_4dcbc28e, 2025-12-02; copied: 2 pasted, 0 lifted into archive notes |
| gemini-1784-prompt | gemini:1784 response → gemini:1784 prompt | Gemini web (lineage), thread th_4e15130e, 2026-02-13; not copied |
| gemini-253-prompt | gemini:253 response → gemini:253 prompt | Gemini web (lineage), thread th_4ea0521d, 2025-12-06; not copied |
| gemini-254-prompt | gemini:254 response → gemini:254 prompt | Gemini web (lineage), thread th_4ea0521d, 2025-12-06; not copied |
| gemini-255-prompt | gemini:255 response → gemini:255 prompt | Gemini web (lineage), thread th_4ea0521d, 2025-12-06; not copied |
| gemini-1-prompt | gemini:1 response → gemini:1 prompt | Gemini web (lineage), thread th_4f412360, 2025-09-11; not copied |
| gemini-2-prompt | gemini:2 response → gemini:2 prompt | Gemini web (lineage), thread th_4f412360, 2025-09-11; not copied |
| gemini-3-prompt | gemini:3 response → gemini:3 prompt | Gemini web (lineage), thread th_4f412360, 2025-09-11; not copied |
| gemini-4-prompt | gemini:4 response → gemini:4 prompt | Gemini web (lineage), thread th_4f412360, 2025-09-11; not copied |
| gemini-5-prompt | gemini:5 response → gemini:5 prompt | Gemini web (lineage), thread th_4f412360, 2025-09-11; not copied |
| gemini-6-prompt | gemini:6 response → gemini:6 prompt | Gemini web (lineage), thread th_4f412360, 2025-09-11; not copied |
| gemini-7-prompt | gemini:7 response → gemini:7 prompt | Gemini web (lineage), thread th_4f412360, 2025-09-11; not copied |
| gemini-8-prompt | gemini:8 response → gemini:8 prompt | Gemini web (lineage), thread th_4f412360, 2025-09-11; not copied |
| gemini-9-prompt | gemini:9 response → gemini:9 prompt | Gemini web (lineage), thread th_4f412360, 2025-09-11; not copied |
| gemini-2575-prompt | gemini:2575 response → gemini:2575 prompt | Gemini web (lineage), thread th_4fa5eb66, 2026-03-09; not copied |
| gemini-615-prompt | gemini:615 response → gemini:615 prompt | Gemini web (lineage), thread th_506dfe03, 2026-01-10; not copied |
| gemini-616-prompt | gemini:616 response → gemini:616 prompt | Gemini web (lineage), thread th_506dfe03, 2026-01-10; not copied |
| gemini-617-prompt | gemini:617 response → gemini:617 prompt | Gemini web (lineage), thread th_506dfe03, 2026-01-10; not copied |
| gemini-1000-prompt | gemini:1000 response → gemini:1000 prompt | Gemini web (lineage), thread th_513c09f6, 2026-01-24; copied: 1 pasted, 1 lifted into archive notes |
| gemini-150-prompt | gemini:150 response → gemini:150 prompt | Gemini web (lineage), thread th_51792fac, 2025-12-04; not copied |
| gemini-3037-prompt | gemini:3037 response → gemini:3037 prompt | Gemini web (lineage), thread th_528d2e39, 2026-03-31; not copied |
| gemini-3038-prompt | gemini:3038 response → gemini:3038 prompt | Gemini web (lineage), thread th_528d2e39, 2026-03-31; not copied |
| gemini-3039-prompt | gemini:3039 response → gemini:3039 prompt | Gemini web (lineage), thread th_528d2e39, 2026-03-31; not copied |
| gemini-3040-prompt | gemini:3040 response → gemini:3040 prompt | Gemini web (lineage), thread th_528d2e39, 2026-03-31; not copied |
| gemini-3041-prompt | gemini:3041 response → gemini:3041 prompt | Gemini web (lineage), thread th_528d2e39, 2026-03-31; not copied |
| gemini-3042-prompt | gemini:3042 response → gemini:3042 prompt | Gemini web (lineage), thread th_528d2e39, 2026-03-31; not copied |
| gemini-1112-prompt | gemini:1112 response → gemini:1112 prompt | Gemini web (lineage), thread th_52eab3fa, 2026-01-26; copied: 1 pasted, 0 lifted into archive notes |
| gemini-1113-prompt | gemini:1113 response → gemini:1113 prompt | Gemini web (lineage), thread th_52eab3fa, 2026-01-26; copied: 3 pasted, 0 lifted into archive notes |
| gemini-1114-prompt | gemini:1114 response → gemini:1114 prompt | Gemini web (lineage), thread th_52eab3fa, 2026-01-26; copied: 2 pasted, 0 lifted into archive notes |
| gemini-1115-prompt | gemini:1115 response → gemini:1115 prompt | Gemini web (lineage), thread th_52eab3fa, 2026-01-26; copied: 1 pasted, 0 lifted into archive notes |
| gemini-1116-prompt | gemini:1116 response → gemini:1116 prompt | Gemini web (lineage), thread th_52eab3fa, 2026-01-26; not copied |
| gemini-1117-prompt | gemini:1117 response → gemini:1117 prompt | Gemini web (lineage), thread th_52eab3fa, 2026-01-26; not copied |
| gemini-1118-prompt | gemini:1118 response → gemini:1118 prompt | Gemini web (lineage), thread th_52eab3fa, 2026-01-26; not copied |
| gemini-873-prompt | gemini:873 response → gemini:873 prompt | Gemini web (lineage), thread th_53b00f75, 2026-01-20; not copied |
| gemini-874-prompt | gemini:874 response → gemini:874 prompt | Gemini web (lineage), thread th_53b00f75, 2026-01-20; copied: 1 pasted, 0 lifted into archive notes |
| gemini-875-prompt | gemini:875 response → gemini:875 prompt | Gemini web (lineage), thread th_53b00f75, 2026-01-20; not copied |
| gemini-563-prompt | gemini:563 response → gemini:563 prompt | Gemini web (lineage), thread th_5400219f, 2026-01-09; not copied |
| gemini-564-prompt | gemini:564 response → gemini:564 prompt | Gemini web (lineage), thread th_5400219f, 2026-01-09; not copied |
| gemini-565-prompt | gemini:565 response → gemini:565 prompt | Gemini web (lineage), thread th_5400219f, 2026-01-09; not copied |
| gemini-566-prompt | gemini:566 response → gemini:566 prompt | Gemini web (lineage), thread th_5400219f, 2026-01-09; not copied |
| gemini-567-prompt | gemini:567 response → gemini:567 prompt | Gemini web (lineage), thread th_5400219f, 2026-01-09; not copied |
| gemini-1453-prompt | gemini:1453 response → gemini:1453 prompt | Gemini web (lineage), thread th_54150ac4, 2026-02-07; not copied |
| gemini-2383-prompt | gemini:2383 response → gemini:2383 prompt | Gemini web (lineage), thread th_54255ff4, 2026-03-04; not copied |
| gemini-2088-prompt | gemini:2088 response → gemini:2088 prompt | Gemini web (lineage), thread th_544ad2c8, 2026-02-24 to 2026-02-25; not copied |
| gemini-2089-prompt | gemini:2089 response → gemini:2089 prompt | Gemini web (lineage), thread th_544ad2c8, 2026-02-24 to 2026-02-25; not copied |
| gemini-2090-prompt | gemini:2090 response → gemini:2090 prompt | Gemini web (lineage), thread th_544ad2c8, 2026-02-24 to 2026-02-25; not copied |
| gemini-2091-prompt | gemini:2091 response → gemini:2091 prompt | Gemini web (lineage), thread th_544ad2c8, 2026-02-24 to 2026-02-25; not copied |
| gemini-2092-prompt | gemini:2092 response → gemini:2092 prompt | Gemini web (lineage), thread th_544ad2c8, 2026-02-24 to 2026-02-25; not copied |
| gemini-2093-prompt | gemini:2093 response → gemini:2093 prompt | Gemini web (lineage), thread th_544ad2c8, 2026-02-24 to 2026-02-25; not copied |
| gemini-2094-prompt | gemini:2094 response → gemini:2094 prompt | Gemini web (lineage), thread th_544ad2c8, 2026-02-24 to 2026-02-25; not copied |
| gemini-2095-prompt | gemini:2095 response → gemini:2095 prompt | Gemini web (lineage), thread th_544ad2c8, 2026-02-24 to 2026-02-25; not copied |
| gemini-2096-prompt | gemini:2096 response → gemini:2096 prompt | Gemini web (lineage), thread th_544ad2c8, 2026-02-24 to 2026-02-25; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2097-prompt | gemini:2097 response → gemini:2097 prompt | Gemini web (lineage), thread th_544ad2c8, 2026-02-24 to 2026-02-25; not copied |
| gemini-3086-prompt | gemini:3086 response → gemini:3086 prompt | Gemini web (lineage), thread th_5487f5cd, 2026-04-05; not copied |
| gemini-3087-prompt | gemini:3087 response → gemini:3087 prompt | Gemini web (lineage), thread th_5487f5cd, 2026-04-05; not copied |
| gemini-2922-prompt | gemini:2922 response → gemini:2922 prompt | Gemini web (lineage), thread th_555d02c5, 2026-03-24; not copied |
| gemini-2923-prompt | gemini:2923 response → gemini:2923 prompt | Gemini web (lineage), thread th_555d02c5, 2026-03-24; not copied |
| gemini-419-prompt | gemini:419 response → gemini:419 prompt | Gemini web (lineage), thread th_55cd97d5, 2025-12-28; not copied |
| gemini-420-prompt | gemini:420 response → gemini:420 prompt | Gemini web (lineage), thread th_55cd97d5, 2025-12-28; copied: 1 pasted, 0 lifted into archive notes |
| gemini-421-prompt | gemini:421 response → gemini:421 prompt | Gemini web (lineage), thread th_55cd97d5, 2025-12-28; not copied |
| gemini-422-prompt | gemini:422 response → gemini:422 prompt | Gemini web (lineage), thread th_55cd97d5, 2025-12-28; not copied |
| gemini-2591-prompt | gemini:2591 response → gemini:2591 prompt | Gemini web (lineage), thread th_567971b6, 2026-03-09; not copied |
| gemini-2592-prompt | gemini:2592 response → gemini:2592 prompt | Gemini web (lineage), thread th_567971b6, 2026-03-09; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2593-prompt | gemini:2593 response → gemini:2593 prompt | Gemini web (lineage), thread th_567971b6, 2026-03-09; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2594-prompt | gemini:2594 response → gemini:2594 prompt | Gemini web (lineage), thread th_567971b6, 2026-03-09; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2595-prompt | gemini:2595 response → gemini:2595 prompt | Gemini web (lineage), thread th_567971b6, 2026-03-09; copied: 2 pasted, 0 lifted into archive notes |
| gemini-2596-prompt | gemini:2596 response → gemini:2596 prompt | Gemini web (lineage), thread th_567971b6, 2026-03-09; not copied |
| gemini-2597-prompt | gemini:2597 response → gemini:2597 prompt | Gemini web (lineage), thread th_567971b6, 2026-03-09; copied: 4 pasted, 0 lifted into archive notes |
| gemini-2598-prompt | gemini:2598 response → gemini:2598 prompt | Gemini web (lineage), thread th_567971b6, 2026-03-09; not copied |
| gemini-2599-prompt | gemini:2599 response → gemini:2599 prompt | Gemini web (lineage), thread th_567971b6, 2026-03-09; not copied |
| gemini-2600-prompt | gemini:2600 response → gemini:2600 prompt | Gemini web (lineage), thread th_567971b6, 2026-03-09; not copied |
| gemini-2601-prompt | gemini:2601 response → gemini:2601 prompt | Gemini web (lineage), thread th_567971b6, 2026-03-09; not copied |
| gemini-2602-prompt | gemini:2602 response → gemini:2602 prompt | Gemini web (lineage), thread th_567971b6, 2026-03-09; not copied |
| gemini-2603-prompt | gemini:2603 response → gemini:2603 prompt | Gemini web (lineage), thread th_567971b6, 2026-03-09; not copied |
| gemini-2604-prompt | gemini:2604 response → gemini:2604 prompt | Gemini web (lineage), thread th_567971b6, 2026-03-09; not copied |
| gemini-2605-prompt | gemini:2605 response → gemini:2605 prompt | Gemini web (lineage), thread th_567971b6, 2026-03-09; not copied |
| gemini-2606-prompt | gemini:2606 response → gemini:2606 prompt | Gemini web (lineage), thread th_567971b6, 2026-03-09; copied: 1 pasted, 0 lifted into archive notes |
| gemini-687-prompt | gemini:687 response → gemini:687 prompt | Gemini web (lineage), thread th_56afc48c, 2026-01-13; copied: 0 pasted, 1 lifted into archive notes |
| gemini-3259-prompt | gemini:3259 response → gemini:3259 prompt | Gemini web (lineage), thread th_56b3b342, 2026-06-08; not copied |
| gemini-2729-prompt | gemini:2729 response → gemini:2729 prompt | Gemini web (lineage), thread th_5743d3d4, 2026-03-17; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2730-prompt | gemini:2730 response → gemini:2730 prompt | Gemini web (lineage), thread th_5743d3d4, 2026-03-17; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2731-prompt | gemini:2731 response → gemini:2731 prompt | Gemini web (lineage), thread th_5743d3d4, 2026-03-17; not copied |
| gemini-2732-prompt | gemini:2732 response → gemini:2732 prompt | Gemini web (lineage), thread th_5743d3d4, 2026-03-17; not copied |
| gemini-1484-prompt | gemini:1484 response → gemini:1484 prompt | Gemini web (lineage), thread th_58128247, 2026-02-08; not copied |
| gemini-1485-prompt | gemini:1485 response → gemini:1485 prompt | Gemini web (lineage), thread th_58128247, 2026-02-08; not copied |
| gemini-1486-prompt | gemini:1486 response → gemini:1486 prompt | Gemini web (lineage), thread th_58128247, 2026-02-08; not copied |
| gemini-1487-prompt | gemini:1487 response → gemini:1487 prompt | Gemini web (lineage), thread th_58128247, 2026-02-08; copied: 3 pasted, 0 lifted into archive notes |
| gemini-1488-prompt | gemini:1488 response → gemini:1488 prompt | Gemini web (lineage), thread th_58128247, 2026-02-08; not copied |
| gemini-1489-prompt | gemini:1489 response → gemini:1489 prompt | Gemini web (lineage), thread th_58128247, 2026-02-08; copied: 1 pasted, 0 lifted into archive notes |
| gemini-1490-prompt | gemini:1490 response → gemini:1490 prompt | Gemini web (lineage), thread th_58128247, 2026-02-08; not copied |
| gemini-1491-prompt | gemini:1491 response → gemini:1491 prompt | Gemini web (lineage), thread th_58128247, 2026-02-08; copied: 2 pasted, 0 lifted into archive notes |
| gemini-1492-prompt | gemini:1492 response → gemini:1492 prompt | Gemini web (lineage), thread th_58128247, 2026-02-08; not copied |
| gemini-1493-prompt | gemini:1493 response → gemini:1493 prompt | Gemini web (lineage), thread th_58128247, 2026-02-08; not copied |
| gemini-1494-prompt | gemini:1494 response → gemini:1494 prompt | Gemini web (lineage), thread th_58128247, 2026-02-08; not copied |
| gemini-1495-prompt | gemini:1495 response → gemini:1495 prompt | Gemini web (lineage), thread th_58128247, 2026-02-08; not copied |
| gemini-1496-prompt | gemini:1496 response → gemini:1496 prompt | Gemini web (lineage), thread th_58128247, 2026-02-08; not copied |
| gemini-1497-prompt | gemini:1497 response → gemini:1497 prompt | Gemini web (lineage), thread th_58128247, 2026-02-08; not copied |
| gemini-1498-prompt | gemini:1498 response → gemini:1498 prompt | Gemini web (lineage), thread th_58128247, 2026-02-08; not copied |
| gemini-1499-prompt | gemini:1499 response → gemini:1499 prompt | Gemini web (lineage), thread th_58128247, 2026-02-08; copied: 0 pasted, 1 lifted into archive notes |
| gemini-1500-prompt | gemini:1500 response → gemini:1500 prompt | Gemini web (lineage), thread th_58128247, 2026-02-08; not copied |
| gemini-1501-prompt | gemini:1501 response → gemini:1501 prompt | Gemini web (lineage), thread th_58128247, 2026-02-08; not copied |
| gemini-1502-prompt | gemini:1502 response → gemini:1502 prompt | Gemini web (lineage), thread th_58128247, 2026-02-08; not copied |
| gemini-1503-prompt | gemini:1503 response → gemini:1503 prompt | Gemini web (lineage), thread th_58128247, 2026-02-08; copied: 2 pasted, 0 lifted into archive notes |
| gemini-1504-prompt | gemini:1504 response → gemini:1504 prompt | Gemini web (lineage), thread th_58128247, 2026-02-08; copied: 1 pasted, 1 lifted into archive notes |
| gemini-1505-prompt | gemini:1505 response → gemini:1505 prompt | Gemini web (lineage), thread th_58128247, 2026-02-08; not copied |
| gemini-1506-prompt | gemini:1506 response → gemini:1506 prompt | Gemini web (lineage), thread th_58128247, 2026-02-08; not copied |
| gemini-1507-prompt | gemini:1507 response → gemini:1507 prompt | Gemini web (lineage), thread th_58128247, 2026-02-08; not copied |
| gemini-1508-prompt | gemini:1508 response → gemini:1508 prompt | Gemini web (lineage), thread th_58128247, 2026-02-08; copied: 1 pasted, 0 lifted into archive notes |
| gemini-1509-prompt | gemini:1509 response → gemini:1509 prompt | Gemini web (lineage), thread th_58128247, 2026-02-08; not copied |
| gemini-1510-prompt | gemini:1510 response → gemini:1510 prompt | Gemini web (lineage), thread th_58128247, 2026-02-08; copied: 1 pasted, 0 lifted into archive notes |
| gemini-1511-prompt | gemini:1511 response → gemini:1511 prompt | Gemini web (lineage), thread th_58128247, 2026-02-08; not copied |
| gemini-1512-prompt | gemini:1512 response → gemini:1512 prompt | Gemini web (lineage), thread th_58128247, 2026-02-08; not copied |
| gemini-1513-prompt | gemini:1513 response → gemini:1513 prompt | Gemini web (lineage), thread th_58128247, 2026-02-08; not copied |
| gemini-1514-prompt | gemini:1514 response → gemini:1514 prompt | Gemini web (lineage), thread th_58128247, 2026-02-08; not copied |
| gemini-1515-prompt | gemini:1515 response → gemini:1515 prompt | Gemini web (lineage), thread th_58128247, 2026-02-08; copied: 0 pasted, 1 lifted into archive notes |
| gemini-1516-prompt | gemini:1516 response → gemini:1516 prompt | Gemini web (lineage), thread th_58128247, 2026-02-08; not copied |
| gemini-2233-prompt | gemini:2233 response → gemini:2233 prompt | Gemini web (lineage), thread th_585efba9, 2026-02-27; not copied |
| gemini-2234-prompt | gemini:2234 response → gemini:2234 prompt | Gemini web (lineage), thread th_585efba9, 2026-02-27; not copied |
| gemini-2235-prompt | gemini:2235 response → gemini:2235 prompt | Gemini web (lineage), thread th_585efba9, 2026-02-27; not copied |
| gemini-2236-prompt | gemini:2236 response → gemini:2236 prompt | Gemini web (lineage), thread th_585efba9, 2026-02-27; not copied |
| gemini-2237-prompt | gemini:2237 response → gemini:2237 prompt | Gemini web (lineage), thread th_585efba9, 2026-02-27; not copied |
| gemini-2238-prompt | gemini:2238 response → gemini:2238 prompt | Gemini web (lineage), thread th_585efba9, 2026-02-27; not copied |
| gemini-2239-prompt | gemini:2239 response → gemini:2239 prompt | Gemini web (lineage), thread th_585efba9, 2026-02-27; not copied |
| gemini-2240-prompt | gemini:2240 response → gemini:2240 prompt | Gemini web (lineage), thread th_585efba9, 2026-02-27; not copied |
| gemini-2241-prompt | gemini:2241 response → gemini:2241 prompt | Gemini web (lineage), thread th_585efba9, 2026-02-27; not copied |
| gemini-2242-prompt | gemini:2242 response → gemini:2242 prompt | Gemini web (lineage), thread th_585efba9, 2026-02-27; not copied |
| gemini-2243-prompt | gemini:2243 response → gemini:2243 prompt | Gemini web (lineage), thread th_585efba9, 2026-02-27; not copied |
| gemini-2244-prompt | gemini:2244 response → gemini:2244 prompt | Gemini web (lineage), thread th_585efba9, 2026-02-27; not copied |
| gemini-2245-prompt | gemini:2245 response → gemini:2245 prompt | Gemini web (lineage), thread th_585efba9, 2026-02-27; not copied |
| gemini-2246-prompt | gemini:2246 response → gemini:2246 prompt | Gemini web (lineage), thread th_585efba9, 2026-02-27; not copied |
| gemini-1271-prompt | gemini:1271 response → gemini:1271 prompt | Gemini web (lineage), thread th_586cc4ce, 2026-02-01; copied: 4 pasted, 0 lifted into archive notes |
| gemini-1272-prompt | gemini:1272 response → gemini:1272 prompt | Gemini web (lineage), thread th_586cc4ce, 2026-02-01; not copied |
| gemini-1273-prompt | gemini:1273 response → gemini:1273 prompt | Gemini web (lineage), thread th_586cc4ce, 2026-02-01; not copied |
| gemini-1274-prompt | gemini:1274 response → gemini:1274 prompt | Gemini web (lineage), thread th_586cc4ce, 2026-02-01; not copied |
| gemini-1275-prompt | gemini:1275 response → gemini:1275 prompt | Gemini web (lineage), thread th_586cc4ce, 2026-02-01; not copied |
| gemini-1276-prompt | gemini:1276 response → gemini:1276 prompt | Gemini web (lineage), thread th_586cc4ce, 2026-02-01; not copied |
| gemini-1277-prompt | gemini:1277 response → gemini:1277 prompt | Gemini web (lineage), thread th_586cc4ce, 2026-02-01; not copied |
| gemini-1278-prompt | gemini:1278 response → gemini:1278 prompt | Gemini web (lineage), thread th_586cc4ce, 2026-02-01; not copied |
| gemini-1279-prompt | gemini:1279 response → gemini:1279 prompt | Gemini web (lineage), thread th_586cc4ce, 2026-02-01; not copied |
| gemini-1280-prompt | gemini:1280 response → gemini:1280 prompt | Gemini web (lineage), thread th_586cc4ce, 2026-02-01; not copied |
| gemini-1281-prompt | gemini:1281 response → gemini:1281 prompt | Gemini web (lineage), thread th_586cc4ce, 2026-02-01; not copied |
| gemini-1282-prompt | gemini:1282 response → gemini:1282 prompt | Gemini web (lineage), thread th_586cc4ce, 2026-02-01; not copied |
| gemini-1283-prompt | gemini:1283 response → gemini:1283 prompt | Gemini web (lineage), thread th_586cc4ce, 2026-02-01; not copied |
| gemini-1284-prompt | gemini:1284 response → gemini:1284 prompt | Gemini web (lineage), thread th_586cc4ce, 2026-02-01; not copied |
| gemini-1285-prompt | gemini:1285 response → gemini:1285 prompt | Gemini web (lineage), thread th_586cc4ce, 2026-02-01; not copied |
| gemini-1901-prompt | gemini:1901 response → gemini:1901 prompt | Gemini web (lineage), thread th_586cf1e4, 2026-02-18; not copied |
| gemini-1902-prompt | gemini:1902 response → gemini:1902 prompt | Gemini web (lineage), thread th_586cf1e4, 2026-02-18; not copied |
| gemini-1903-prompt | gemini:1903 response → gemini:1903 prompt | Gemini web (lineage), thread th_586cf1e4, 2026-02-18; not copied |
| gemini-122-prompt | gemini:122 response → gemini:122 prompt | Gemini web (lineage), thread th_588fd775, 2025-12-03; not copied |
| gemini-2178-prompt | gemini:2178 response → gemini:2178 prompt | Gemini web (lineage), thread th_58bdd50b, 2026-02-26; not copied |
| gemini-3160-prompt | gemini:3160 response → gemini:3160 prompt | Gemini web (lineage), thread th_58dbd94d, 2026-04-14; not copied |
| gemini-3161-prompt | gemini:3161 response → gemini:3161 prompt | Gemini web (lineage), thread th_58dbd94d, 2026-04-14; not copied |
| gemini-2694-prompt | gemini:2694 response → gemini:2694 prompt | Gemini web (lineage), thread th_58ec2bc7, 2026-03-13; copied: 2 pasted, 0 lifted into archive notes |
| gemini-2695-prompt | gemini:2695 response → gemini:2695 prompt | Gemini web (lineage), thread th_58ec2bc7, 2026-03-13; not copied |
| gemini-627-prompt | gemini:627 response → gemini:627 prompt | Gemini web (lineage), thread th_58ff867c, 2026-01-10; not copied |
| gemini-628-prompt | gemini:628 response → gemini:628 prompt | Gemini web (lineage), thread th_58ff867c, 2026-01-10; not copied |
| gemini-2310-prompt | gemini:2310 response → gemini:2310 prompt | Gemini web (lineage), thread th_59217650, 2026-03-01; not copied |
| gemini-2311-prompt | gemini:2311 response → gemini:2311 prompt | Gemini web (lineage), thread th_59217650, 2026-03-01; not copied |
| gemini-2312-prompt | gemini:2312 response → gemini:2312 prompt | Gemini web (lineage), thread th_59217650, 2026-03-01; not copied |
| gemini-2313-prompt | gemini:2313 response → gemini:2313 prompt | Gemini web (lineage), thread th_59217650, 2026-03-01; not copied |
| gemini-2314-prompt | gemini:2314 response → gemini:2314 prompt | Gemini web (lineage), thread th_59217650, 2026-03-01; not copied |
| gemini-3011-prompt | gemini:3011 response → gemini:3011 prompt | Gemini web (lineage), thread th_59911c3d, 2026-03-28; not copied |
| gemini-1524-prompt | gemini:1524 response → gemini:1524 prompt | Gemini web (lineage), thread th_5a867fcb, 2026-02-08; copied: 3 pasted, 0 lifted into archive notes |
| gemini-2517-prompt | gemini:2517 response → gemini:2517 prompt | Gemini web (lineage), thread th_5b1daac3, 2026-03-08; not copied |
| gemini-2518-prompt | gemini:2518 response → gemini:2518 prompt | Gemini web (lineage), thread th_5b1daac3, 2026-03-08; copied: 1 pasted, 0 lifted into archive notes |
| gemini-815-prompt | gemini:815 response → gemini:815 prompt | Gemini web (lineage), thread th_5b24a39b, 2026-01-18; not copied |
| gemini-1517-prompt | gemini:1517 response → gemini:1517 prompt | Gemini web (lineage), thread th_5b6dd39e, 2026-02-08; not copied |
| gemini-2551-prompt | gemini:2551 response → gemini:2551 prompt | Gemini web (lineage), thread th_5bb015ee, 2026-03-08; not copied |
| gemini-2150-prompt | gemini:2150 response → gemini:2150 prompt | Gemini web (lineage), thread th_5bc7eca5, 2026-02-26; not copied |
| gemini-2151-prompt | gemini:2151 response → gemini:2151 prompt | Gemini web (lineage), thread th_5bc7eca5, 2026-02-26; not copied |
| gemini-2152-prompt | gemini:2152 response → gemini:2152 prompt | Gemini web (lineage), thread th_5bc7eca5, 2026-02-26; not copied |
| gemini-2153-prompt | gemini:2153 response → gemini:2153 prompt | Gemini web (lineage), thread th_5bc7eca5, 2026-02-26; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2154-prompt | gemini:2154 response → gemini:2154 prompt | Gemini web (lineage), thread th_5bc7eca5, 2026-02-26; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2155-prompt | gemini:2155 response → gemini:2155 prompt | Gemini web (lineage), thread th_5bc7eca5, 2026-02-26; not copied |
| gemini-2156-prompt | gemini:2156 response → gemini:2156 prompt | Gemini web (lineage), thread th_5bc7eca5, 2026-02-26; not copied |
| gemini-2157-prompt | gemini:2157 response → gemini:2157 prompt | Gemini web (lineage), thread th_5bc7eca5, 2026-02-26; not copied |
| gemini-2158-prompt | gemini:2158 response → gemini:2158 prompt | Gemini web (lineage), thread th_5bc7eca5, 2026-02-26; not copied |
| gemini-2159-prompt | gemini:2159 response → gemini:2159 prompt | Gemini web (lineage), thread th_5bc7eca5, 2026-02-26; not copied |
| gemini-2160-prompt | gemini:2160 response → gemini:2160 prompt | Gemini web (lineage), thread th_5bc7eca5, 2026-02-26; not copied |
| gemini-2161-prompt | gemini:2161 response → gemini:2161 prompt | Gemini web (lineage), thread th_5bc7eca5, 2026-02-26; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2162-prompt | gemini:2162 response → gemini:2162 prompt | Gemini web (lineage), thread th_5bc7eca5, 2026-02-26; copied: 2 pasted, 0 lifted into archive notes |
| gemini-2163-prompt | gemini:2163 response → gemini:2163 prompt | Gemini web (lineage), thread th_5bc7eca5, 2026-02-26; not copied |
| gemini-2164-prompt | gemini:2164 response → gemini:2164 prompt | Gemini web (lineage), thread th_5bc7eca5, 2026-02-26; not copied |
| gemini-2165-prompt | gemini:2165 response → gemini:2165 prompt | Gemini web (lineage), thread th_5bc7eca5, 2026-02-26; not copied |
| gemini-2166-prompt | gemini:2166 response → gemini:2166 prompt | Gemini web (lineage), thread th_5bc7eca5, 2026-02-26; copied: 2 pasted, 0 lifted into archive notes |
| gemini-2167-prompt | gemini:2167 response → gemini:2167 prompt | Gemini web (lineage), thread th_5bc7eca5, 2026-02-26; copied: 2 pasted, 0 lifted into archive notes |
| gemini-2168-prompt | gemini:2168 response → gemini:2168 prompt | Gemini web (lineage), thread th_5bc7eca5, 2026-02-26; not copied |
| gemini-2169-prompt | gemini:2169 response → gemini:2169 prompt | Gemini web (lineage), thread th_5bc7eca5, 2026-02-26; copied: 2 pasted, 0 lifted into archive notes |
| gemini-2170-prompt | gemini:2170 response → gemini:2170 prompt | Gemini web (lineage), thread th_5bc7eca5, 2026-02-26; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2171-prompt | gemini:2171 response → gemini:2171 prompt | Gemini web (lineage), thread th_5bc7eca5, 2026-02-26; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2172-prompt | gemini:2172 response → gemini:2172 prompt | Gemini web (lineage), thread th_5bc7eca5, 2026-02-26; not copied |
| gemini-2173-prompt | gemini:2173 response → gemini:2173 prompt | Gemini web (lineage), thread th_5bc7eca5, 2026-02-26; not copied |
| gemini-2174-prompt | gemini:2174 response → gemini:2174 prompt | Gemini web (lineage), thread th_5bc7eca5, 2026-02-26; not copied |
| gemini-2175-prompt | gemini:2175 response → gemini:2175 prompt | Gemini web (lineage), thread th_5bc7eca5, 2026-02-26; not copied |
| gemini-2176-prompt | gemini:2176 response → gemini:2176 prompt | Gemini web (lineage), thread th_5bc7eca5, 2026-02-26; not copied |
| gemini-2177-prompt | gemini:2177 response → gemini:2177 prompt | Gemini web (lineage), thread th_5bc7eca5, 2026-02-26; not copied |
| gemini-811-prompt | gemini:811 response → gemini:811 prompt | Gemini web (lineage), thread th_5c52517e, 2026-01-18; copied: 1 pasted, 0 lifted into archive notes |
| gemini-424-prompt | gemini:424 response → gemini:424 prompt | Gemini web (lineage), thread th_5c645ff4, 2025-12-28; not copied |
| gemini-2997-prompt | gemini:2997 response → gemini:2997 prompt | Gemini web (lineage), thread th_5c6fc131, 2026-03-26; not copied |
| gemini-2998-prompt | gemini:2998 response → gemini:2998 prompt | Gemini web (lineage), thread th_5c6fc131, 2026-03-26; not copied |
| gemini-2999-prompt | gemini:2999 response → gemini:2999 prompt | Gemini web (lineage), thread th_5c6fc131, 2026-03-26; not copied |
| gemini-3000-prompt | gemini:3000 response → gemini:3000 prompt | Gemini web (lineage), thread th_5c6fc131, 2026-03-26; not copied |
| gemini-1019-prompt | gemini:1019 response → gemini:1019 prompt | Gemini web (lineage), thread th_5c9480b5, 2026-01-24; not copied |
| gemini-2636-prompt | gemini:2636 response → gemini:2636 prompt | Gemini web (lineage), thread th_5d032c65, 2026-03-10; not copied |
| gemini-2637-prompt | gemini:2637 response → gemini:2637 prompt | Gemini web (lineage), thread th_5d032c65, 2026-03-10; copied: 1 pasted, 0 lifted into archive notes |
| gemini-214-prompt | gemini:214 response → gemini:214 prompt | Gemini web (lineage), thread th_5d6fcdb8, 2025-12-05; copied: 0 pasted, 1 lifted into archive notes |
| gemini-16-prompt | gemini:16 response → gemini:16 prompt | Gemini web (lineage), thread th_5dd17dd1, 2025-11-11; not copied |
| gemini-17-prompt | gemini:17 response → gemini:17 prompt | Gemini web (lineage), thread th_5dd17dd1, 2025-11-11; not copied |
| gemini-18-prompt | gemini:18 response → gemini:18 prompt | Gemini web (lineage), thread th_5dd17dd1, 2025-11-11; not copied |
| gemini-19-prompt | gemini:19 response → gemini:19 prompt | Gemini web (lineage), thread th_5dd17dd1, 2025-11-11; not copied |
| gemini-20-prompt | gemini:20 response → gemini:20 prompt | Gemini web (lineage), thread th_5dd17dd1, 2025-11-11; not copied |
| gemini-1974-prompt | gemini:1974 response → gemini:1974 prompt | Gemini web (lineage), thread th_5e0ce2b9, 2026-02-20; not copied |
| gemini-1678-prompt | gemini:1678 response → gemini:1678 prompt | Gemini web (lineage), thread th_5e0f9e66, 2026-02-10; not copied |
| gemini-1679-prompt | gemini:1679 response → gemini:1679 prompt | Gemini web (lineage), thread th_5e0f9e66, 2026-02-10; not copied |
| gemini-1762-prompt | gemini:1762 response → gemini:1762 prompt | Gemini web (lineage), thread th_5ef9b448, 2026-02-12; not copied |
| gemini-2846-prompt | gemini:2846 response → gemini:2846 prompt | Gemini web (lineage), thread th_5f1112d5, 2026-03-22; not copied |
| gemini-234-prompt | gemini:234 response → gemini:234 prompt | Gemini web (lineage), thread th_5f1816df, 2025-12-05; not copied |
| gemini-2889-prompt | gemini:2889 response → gemini:2889 prompt | Gemini web (lineage), thread th_5f3ac6b8, 2026-03-23; not copied |
| gemini-2890-prompt | gemini:2890 response → gemini:2890 prompt | Gemini web (lineage), thread th_5f3ac6b8, 2026-03-23; copied: 4 pasted, 0 lifted into archive notes |
| gemini-2891-prompt | gemini:2891 response → gemini:2891 prompt | Gemini web (lineage), thread th_5f3ac6b8, 2026-03-23; not copied |
| gemini-2892-prompt | gemini:2892 response → gemini:2892 prompt | Gemini web (lineage), thread th_5f3ac6b8, 2026-03-23; not copied |
| gemini-2893-prompt | gemini:2893 response → gemini:2893 prompt | Gemini web (lineage), thread th_5f3ac6b8, 2026-03-23; not copied |
| gemini-2894-prompt | gemini:2894 response → gemini:2894 prompt | Gemini web (lineage), thread th_5f3ac6b8, 2026-03-23; not copied |
| gemini-2895-prompt | gemini:2895 response → gemini:2895 prompt | Gemini web (lineage), thread th_5f3ac6b8, 2026-03-23; not copied |
| gemini-2543-prompt | gemini:2543 response → gemini:2543 prompt | Gemini web (lineage), thread th_5f5e3735, 2026-03-08; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2511-prompt | gemini:2511 response → gemini:2511 prompt | Gemini web (lineage), thread th_5fa40fac, 2026-03-06; not copied |
| gemini-2512-prompt | gemini:2512 response → gemini:2512 prompt | Gemini web (lineage), thread th_5fa40fac, 2026-03-06; not copied |
| gemini-2513-prompt | gemini:2513 response → gemini:2513 prompt | Gemini web (lineage), thread th_5fa40fac, 2026-03-06; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2546-prompt | gemini:2546 response → gemini:2546 prompt | Gemini web (lineage), thread th_5fc0bb5b, 2026-03-08; not copied |
| gemini-2547-prompt | gemini:2547 response → gemini:2547 prompt | Gemini web (lineage), thread th_5fc0bb5b, 2026-03-08; not copied |
| gemini-2548-prompt | gemini:2548 response → gemini:2548 prompt | Gemini web (lineage), thread th_5fc0bb5b, 2026-03-08; not copied |
| gemini-2549-prompt | gemini:2549 response → gemini:2549 prompt | Gemini web (lineage), thread th_5fc0bb5b, 2026-03-08; not copied |
| gemini-2856-prompt | gemini:2856 response → gemini:2856 prompt | Gemini web (lineage), thread th_609613ad, 2026-03-23; not copied |
| gemini-2857-prompt | gemini:2857 response → gemini:2857 prompt | Gemini web (lineage), thread th_609613ad, 2026-03-23; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2858-prompt | gemini:2858 response → gemini:2858 prompt | Gemini web (lineage), thread th_609613ad, 2026-03-23; not copied |
| gemini-404-prompt | gemini:404 response → gemini:404 prompt | Gemini web (lineage), thread th_60c45a72, 2025-12-28; copied: 8 pasted, 0 lifted into archive notes |
| gemini-405-prompt | gemini:405 response → gemini:405 prompt | Gemini web (lineage), thread th_60c45a72, 2025-12-28; copied: 3 pasted, 0 lifted into archive notes |
| gemini-406-prompt | gemini:406 response → gemini:406 prompt | Gemini web (lineage), thread th_60c45a72, 2025-12-28; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2662-prompt | gemini:2662 response → gemini:2662 prompt | Gemini web (lineage), thread th_60c7edcd, 2026-03-11; not copied |
| gemini-1245-prompt | gemini:1245 response → gemini:1245 prompt | Gemini web (lineage), thread th_61bc039c, 2026-01-30; copied: 2 pasted, 0 lifted into archive notes |
| gemini-1246-prompt | gemini:1246 response → gemini:1246 prompt | Gemini web (lineage), thread th_61bc039c, 2026-01-30; copied: 14 pasted, 0 lifted into archive notes |
| gemini-1247-prompt | gemini:1247 response → gemini:1247 prompt | Gemini web (lineage), thread th_61bc039c, 2026-01-30; copied: 4 pasted, 1 lifted into archive notes |
| gemini-1248-prompt | gemini:1248 response → gemini:1248 prompt | Gemini web (lineage), thread th_61bc039c, 2026-01-30; copied: 4 pasted, 0 lifted into archive notes |
| gemini-340-prompt | gemini:340 response → gemini:340 prompt | Gemini web (lineage), thread th_61c076c1, 2025-12-08; not copied |
| gemini-341-prompt | gemini:341 response → gemini:341 prompt | Gemini web (lineage), thread th_61c076c1, 2025-12-08; not copied |
| gemini-343-prompt | gemini:343 response → gemini:343 prompt | Gemini web (lineage), thread th_61c076c1, 2025-12-08; copied: 2 pasted, 0 lifted into archive notes |
| gemini-344-prompt | gemini:344 response → gemini:344 prompt | Gemini web (lineage), thread th_61c076c1, 2025-12-08; not copied |
| gemini-346-prompt | gemini:346 response → gemini:346 prompt | Gemini web (lineage), thread th_61c076c1, 2025-12-08; not copied |
| gemini-298-prompt | gemini:298 response → gemini:298 prompt | Gemini web (lineage), thread th_62217078, 2025-12-08; copied: 8 pasted, 0 lifted into archive notes |
| gemini-299-prompt | gemini:299 response → gemini:299 prompt | Gemini web (lineage), thread th_62217078, 2025-12-08; copied: 5 pasted, 1 lifted into archive notes |
| gemini-300-prompt | gemini:300 response → gemini:300 prompt | Gemini web (lineage), thread th_62217078, 2025-12-08; copied: 1 pasted, 0 lifted into archive notes |
| gemini-301-prompt | gemini:301 response → gemini:301 prompt | Gemini web (lineage), thread th_62217078, 2025-12-08; not copied |
| gemini-302-prompt | gemini:302 response → gemini:302 prompt | Gemini web (lineage), thread th_62217078, 2025-12-08; copied: 1 pasted, 0 lifted into archive notes |
| gemini-303-prompt | gemini:303 response → gemini:303 prompt | Gemini web (lineage), thread th_62217078, 2025-12-08; copied: 3 pasted, 0 lifted into archive notes |
| gemini-304-prompt | gemini:304 response → gemini:304 prompt | Gemini web (lineage), thread th_62217078, 2025-12-08; not copied |
| gemini-305-prompt | gemini:305 response → gemini:305 prompt | Gemini web (lineage), thread th_62217078, 2025-12-08; copied: 3 pasted, 0 lifted into archive notes |
| gemini-2397-prompt | gemini:2397 response → gemini:2397 prompt | Gemini web (lineage), thread th_627acd89, 2026-03-04 to 2026-03-05; not copied |
| gemini-2398-prompt | gemini:2398 response → gemini:2398 prompt | Gemini web (lineage), thread th_627acd89, 2026-03-04 to 2026-03-05; not copied |
| gemini-1596-prompt | gemini:1596 response → gemini:1596 prompt | Gemini web (lineage), thread th_629e21f0, 2026-02-10; not copied |
| gemini-593-prompt | gemini:593 response → gemini:593 prompt | Gemini web (lineage), thread th_62b4525c, 2026-01-10; not copied |
| gemini-594-prompt | gemini:594 response → gemini:594 prompt | Gemini web (lineage), thread th_62b4525c, 2026-01-10; not copied |
| gemini-595-prompt | gemini:595 response → gemini:595 prompt | Gemini web (lineage), thread th_62b4525c, 2026-01-10; not copied |
| gemini-596-prompt | gemini:596 response → gemini:596 prompt | Gemini web (lineage), thread th_62b4525c, 2026-01-10; not copied |
| gemini-597-prompt | gemini:597 response → gemini:597 prompt | Gemini web (lineage), thread th_62b4525c, 2026-01-10; not copied |
| gemini-3104-prompt | gemini:3104 response → gemini:3104 prompt | Gemini web (lineage), thread th_62cd0284, 2026-04-08; not copied |
| gemini-1393-prompt | gemini:1393 response → gemini:1393 prompt | Gemini web (lineage), thread th_62e1c8fa, 2026-02-06; not copied |
| gemini-1394-prompt | gemini:1394 response → gemini:1394 prompt | Gemini web (lineage), thread th_62e1c8fa, 2026-02-06; copied: 1 pasted, 0 lifted into archive notes |
| gemini-1395-prompt | gemini:1395 response → gemini:1395 prompt | Gemini web (lineage), thread th_62e1c8fa, 2026-02-06; copied: 1 pasted, 0 lifted into archive notes |
| gemini-173-prompt | gemini:173 response → gemini:173 prompt | Gemini web (lineage), thread th_62f8a012, 2025-12-04; not copied |
| gemini-212-prompt | gemini:212 response → gemini:212 prompt | Gemini web (lineage), thread th_62f9de9d, 2025-12-05; not copied |
| gemini-213-prompt | gemini:213 response → gemini:213 prompt | Gemini web (lineage), thread th_62f9de9d, 2025-12-05; not copied |
| gemini-1908-prompt | gemini:1908 response → gemini:1908 prompt | Gemini web (lineage), thread th_63008c62, 2026-02-19; not copied |
| gemini-1909-prompt | gemini:1909 response → gemini:1909 prompt | Gemini web (lineage), thread th_63008c62, 2026-02-19; not copied |
| gemini-1910-prompt | gemini:1910 response → gemini:1910 prompt | Gemini web (lineage), thread th_63008c62, 2026-02-19; not copied |
| gemini-1911-prompt | gemini:1911 response → gemini:1911 prompt | Gemini web (lineage), thread th_63008c62, 2026-02-19; not copied |
| gemini-1912-prompt | gemini:1912 response → gemini:1912 prompt | Gemini web (lineage), thread th_63008c62, 2026-02-19; not copied |
| gemini-1913-prompt | gemini:1913 response → gemini:1913 prompt | Gemini web (lineage), thread th_63008c62, 2026-02-19; not copied |
| gemini-1914-prompt | gemini:1914 response → gemini:1914 prompt | Gemini web (lineage), thread th_63008c62, 2026-02-19; not copied |
| gemini-1915-prompt | gemini:1915 response → gemini:1915 prompt | Gemini web (lineage), thread th_63008c62, 2026-02-19; not copied |
| gemini-1916-prompt | gemini:1916 response → gemini:1916 prompt | Gemini web (lineage), thread th_63008c62, 2026-02-19; not copied |
| gemini-1917-prompt | gemini:1917 response → gemini:1917 prompt | Gemini web (lineage), thread th_63008c62, 2026-02-19; not copied |
| gemini-1918-prompt | gemini:1918 response → gemini:1918 prompt | Gemini web (lineage), thread th_63008c62, 2026-02-19; not copied |
| gemini-1919-prompt | gemini:1919 response → gemini:1919 prompt | Gemini web (lineage), thread th_63008c62, 2026-02-19; not copied |
| gemini-1920-prompt | gemini:1920 response → gemini:1920 prompt | Gemini web (lineage), thread th_63008c62, 2026-02-19; not copied |
| gemini-1921-prompt | gemini:1921 response → gemini:1921 prompt | Gemini web (lineage), thread th_63008c62, 2026-02-19; not copied |
| gemini-1922-prompt | gemini:1922 response → gemini:1922 prompt | Gemini web (lineage), thread th_63008c62, 2026-02-19; not copied |
| gemini-1923-prompt | gemini:1923 response → gemini:1923 prompt | Gemini web (lineage), thread th_63008c62, 2026-02-19; not copied |
| gemini-1924-prompt | gemini:1924 response → gemini:1924 prompt | Gemini web (lineage), thread th_63008c62, 2026-02-19; not copied |
| gemini-1925-prompt | gemini:1925 response → gemini:1925 prompt | Gemini web (lineage), thread th_63008c62, 2026-02-19; not copied |
| gemini-1926-prompt | gemini:1926 response → gemini:1926 prompt | Gemini web (lineage), thread th_63008c62, 2026-02-19; not copied |
| gemini-1927-prompt | gemini:1927 response → gemini:1927 prompt | Gemini web (lineage), thread th_63008c62, 2026-02-19; not copied |
| gemini-1928-prompt | gemini:1928 response → gemini:1928 prompt | Gemini web (lineage), thread th_63008c62, 2026-02-19; not copied |
| gemini-1929-prompt | gemini:1929 response → gemini:1929 prompt | Gemini web (lineage), thread th_63008c62, 2026-02-19; not copied |
| gemini-1930-prompt | gemini:1930 response → gemini:1930 prompt | Gemini web (lineage), thread th_63008c62, 2026-02-19; not copied |
| gemini-1931-prompt | gemini:1931 response → gemini:1931 prompt | Gemini web (lineage), thread th_63008c62, 2026-02-19; not copied |
| gemini-15-prompt | gemini:15 response → gemini:15 prompt | Gemini web (lineage), thread th_63285ce7, 2025-10-25; not copied |
| gemini-197-prompt | gemini:197 response → gemini:197 prompt | Gemini web (lineage), thread th_63392fb0, 2025-12-05; not copied |
| gemini-198-prompt | gemini:198 response → gemini:198 prompt | Gemini web (lineage), thread th_63392fb0, 2025-12-05; not copied |
| gemini-2344-prompt | gemini:2344 response → gemini:2344 prompt | Gemini web (lineage), thread th_63aefed5, 2026-03-03; copied: 1 pasted, 0 lifted into archive notes |
| gemini-579-prompt | gemini:579 response → gemini:579 prompt | Gemini web (lineage), thread th_63af40ef, 2026-01-09 to 2026-01-10; not copied |
| gemini-580-prompt | gemini:580 response → gemini:580 prompt | Gemini web (lineage), thread th_63af40ef, 2026-01-09 to 2026-01-10; not copied |
| gemini-581-prompt | gemini:581 response → gemini:581 prompt | Gemini web (lineage), thread th_63af40ef, 2026-01-09 to 2026-01-10; not copied |
| gemini-582-prompt | gemini:582 response → gemini:582 prompt | Gemini web (lineage), thread th_63af40ef, 2026-01-09 to 2026-01-10; not copied |
| gemini-3001-prompt | gemini:3001 response → gemini:3001 prompt | Gemini web (lineage), thread th_63dcbc5d, 2026-03-26; not copied |
| gemini-3002-prompt | gemini:3002 response → gemini:3002 prompt | Gemini web (lineage), thread th_63dcbc5d, 2026-03-26; not copied |
| gemini-3003-prompt | gemini:3003 response → gemini:3003 prompt | Gemini web (lineage), thread th_63dcbc5d, 2026-03-26; not copied |
| gemini-3052-prompt | gemini:3052 response → gemini:3052 prompt | Gemini web (lineage), thread th_63f2be4a, 2026-04-01; not copied |
| gemini-3053-prompt | gemini:3053 response → gemini:3053 prompt | Gemini web (lineage), thread th_64180052, 2026-04-01; not copied |
| gemini-3054-prompt | gemini:3054 response → gemini:3054 prompt | Gemini web (lineage), thread th_64180052, 2026-04-01; not copied |
| gemini-3055-prompt | gemini:3055 response → gemini:3055 prompt | Gemini web (lineage), thread th_64180052, 2026-04-01; not copied |
| gemini-3056-prompt | gemini:3056 response → gemini:3056 prompt | Gemini web (lineage), thread th_64180052, 2026-04-01; not copied |
| gemini-1270-prompt | gemini:1270 response → gemini:1270 prompt | Gemini web (lineage), thread th_64a593e1, 2026-02-01; copied: 5 pasted, 0 lifted into archive notes |
| gemini-2901-prompt | gemini:2901 response → gemini:2901 prompt | Gemini web (lineage), thread th_64a5fc34, 2026-03-23 to 2026-03-24; not copied |
| gemini-2902-prompt | gemini:2902 response → gemini:2902 prompt | Gemini web (lineage), thread th_64a5fc34, 2026-03-23 to 2026-03-24; not copied |
| gemini-2903-prompt | gemini:2903 response → gemini:2903 prompt | Gemini web (lineage), thread th_64a5fc34, 2026-03-23 to 2026-03-24; not copied |
| gemini-2904-prompt | gemini:2904 response → gemini:2904 prompt | Gemini web (lineage), thread th_64a5fc34, 2026-03-23 to 2026-03-24; not copied |
| gemini-2905-prompt | gemini:2905 response → gemini:2905 prompt | Gemini web (lineage), thread th_64a5fc34, 2026-03-23 to 2026-03-24; not copied |
| gemini-2906-prompt | gemini:2906 response → gemini:2906 prompt | Gemini web (lineage), thread th_64a5fc34, 2026-03-23 to 2026-03-24; copied: 0 pasted, 1 lifted into archive notes |
| gemini-2907-prompt | gemini:2907 response → gemini:2907 prompt | Gemini web (lineage), thread th_64a5fc34, 2026-03-23 to 2026-03-24; not copied |
| gemini-2191-prompt | gemini:2191 response → gemini:2191 prompt | Gemini web (lineage), thread th_64b0358a, 2026-02-26; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2192-prompt | gemini:2192 response → gemini:2192 prompt | Gemini web (lineage), thread th_64b0358a, 2026-02-26; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2193-prompt | gemini:2193 response → gemini:2193 prompt | Gemini web (lineage), thread th_64b0358a, 2026-02-26; not copied |
| gemini-2194-prompt | gemini:2194 response → gemini:2194 prompt | Gemini web (lineage), thread th_64b0358a, 2026-02-26; not copied |
| gemini-2195-prompt | gemini:2195 response → gemini:2195 prompt | Gemini web (lineage), thread th_64b0358a, 2026-02-26; copied: 0 pasted, 1 lifted into archive notes |
| gemini-2196-prompt | gemini:2196 response → gemini:2196 prompt | Gemini web (lineage), thread th_64b0358a, 2026-02-26; not copied |
| gemini-2394-prompt | gemini:2394 response → gemini:2394 prompt | Gemini web (lineage), thread th_65cd8c08, 2026-03-04; copied: 5 pasted, 0 lifted into archive notes |
| gemini-1680-prompt | gemini:1680 response → gemini:1680 prompt | Gemini web (lineage), thread th_65f7598b, 2026-02-10; not copied |
| gemini-1681-prompt | gemini:1681 response → gemini:1681 prompt | Gemini web (lineage), thread th_65f7598b, 2026-02-10; not copied |
| gemini-3156-prompt | gemini:3156 response → gemini:3156 prompt | Gemini web (lineage), thread th_6645dc3d, 2026-04-12; not copied |
| gemini-427-prompt | gemini:427 response → gemini:427 prompt | Gemini web (lineage), thread th_6673adf4, 2025-12-28; not copied |
| gemini-1391-prompt | gemini:1391 response → gemini:1391 prompt | Gemini web (lineage), thread th_67464f66, 2026-02-06; not copied |
| gemini-2803-prompt | gemini:2803 response → gemini:2803 prompt | Gemini web (lineage), thread th_676afb81, 2026-03-21; not copied |
| gemini-2804-prompt | gemini:2804 response → gemini:2804 prompt | Gemini web (lineage), thread th_676afb81, 2026-03-21; not copied |
| gemini-2805-prompt | gemini:2805 response → gemini:2805 prompt | Gemini web (lineage), thread th_676afb81, 2026-03-21; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2806-prompt | gemini:2806 response → gemini:2806 prompt | Gemini web (lineage), thread th_676afb81, 2026-03-21; not copied |
| gemini-2807-prompt | gemini:2807 response → gemini:2807 prompt | Gemini web (lineage), thread th_676afb81, 2026-03-21; not copied |
| gemini-2808-prompt | gemini:2808 response → gemini:2808 prompt | Gemini web (lineage), thread th_676afb81, 2026-03-21; not copied |
| gemini-2809-prompt | gemini:2809 response → gemini:2809 prompt | Gemini web (lineage), thread th_676afb81, 2026-03-21; not copied |
| gemini-2810-prompt | gemini:2810 response → gemini:2810 prompt | Gemini web (lineage), thread th_676afb81, 2026-03-21; not copied |
| gemini-2811-prompt | gemini:2811 response → gemini:2811 prompt | Gemini web (lineage), thread th_676afb81, 2026-03-21; not copied |
| gemini-2812-prompt | gemini:2812 response → gemini:2812 prompt | Gemini web (lineage), thread th_676afb81, 2026-03-21; not copied |
| gemini-2813-prompt | gemini:2813 response → gemini:2813 prompt | Gemini web (lineage), thread th_676afb81, 2026-03-21; not copied |
| gemini-2814-prompt | gemini:2814 response → gemini:2814 prompt | Gemini web (lineage), thread th_676afb81, 2026-03-21; not copied |
| gemini-2848-prompt | gemini:2848 response → gemini:2848 prompt | Gemini web (lineage), thread th_67d68475, 2026-03-22; copied: 3 pasted, 0 lifted into archive notes |
| gemini-418-prompt | gemini:418 response → gemini:418 prompt | Gemini web (lineage), thread th_681edcb0, 2025-12-28; copied: 1 pasted, 0 lifted into archive notes |
| gemini-13-prompt | gemini:13 response → gemini:13 prompt | Gemini web (lineage), thread th_68285de0, 2025-09-29; not copied |
| gemini-14-prompt | gemini:14 response → gemini:14 prompt | Gemini web (lineage), thread th_68285de0, 2025-09-29; not copied |
| gemini-3202-prompt | gemini:3202 response → gemini:3202 prompt | Gemini web (lineage), thread th_68ac3847, 2026-04-18; not copied |
| gemini-3203-prompt | gemini:3203 response → gemini:3203 prompt | Gemini web (lineage), thread th_68ac3847, 2026-04-18; not copied |
| gemini-3204-prompt | gemini:3204 response → gemini:3204 prompt | Gemini web (lineage), thread th_68ac3847, 2026-04-18; not copied |
| gemini-3205-prompt | gemini:3205 response → gemini:3205 prompt | Gemini web (lineage), thread th_68ac3847, 2026-04-18; not copied |
| gemini-3206-prompt | gemini:3206 response → gemini:3206 prompt | Gemini web (lineage), thread th_68ac3847, 2026-04-18; not copied |
| gemini-3207-prompt | gemini:3207 response → gemini:3207 prompt | Gemini web (lineage), thread th_68ac3847, 2026-04-18; not copied |
| gemini-3208-prompt | gemini:3208 response → gemini:3208 prompt | Gemini web (lineage), thread th_68ac3847, 2026-04-18; not copied |
| gemini-3209-prompt | gemini:3209 response → gemini:3209 prompt | Gemini web (lineage), thread th_68ac3847, 2026-04-18; not copied |
| gemini-184-prompt | gemini:184 response → gemini:184 prompt | Gemini web (lineage), thread th_68d8c8a8, 2025-12-04; not copied |
| gemini-2523-prompt | gemini:2523 response → gemini:2523 prompt | Gemini web (lineage), thread th_6903e371, 2026-03-08; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2524-prompt | gemini:2524 response → gemini:2524 prompt | Gemini web (lineage), thread th_6903e371, 2026-03-08; not copied |
| gemini-2525-prompt | gemini:2525 response → gemini:2525 prompt | Gemini web (lineage), thread th_6903e371, 2026-03-08; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2526-prompt | gemini:2526 response → gemini:2526 prompt | Gemini web (lineage), thread th_6903e371, 2026-03-08; copied: 3 pasted, 0 lifted into archive notes |
| gemini-2527-prompt | gemini:2527 response → gemini:2527 prompt | Gemini web (lineage), thread th_6903e371, 2026-03-08; not copied |
| gemini-2528-prompt | gemini:2528 response → gemini:2528 prompt | Gemini web (lineage), thread th_6903e371, 2026-03-08; not copied |
| gemini-2529-prompt | gemini:2529 response → gemini:2529 prompt | Gemini web (lineage), thread th_6903e371, 2026-03-08; copied: 2 pasted, 0 lifted into archive notes |
| gemini-2530-prompt | gemini:2530 response → gemini:2530 prompt | Gemini web (lineage), thread th_6903e371, 2026-03-08; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2531-prompt | gemini:2531 response → gemini:2531 prompt | Gemini web (lineage), thread th_6903e371, 2026-03-08; not copied |
| gemini-2532-prompt | gemini:2532 response → gemini:2532 prompt | Gemini web (lineage), thread th_6903e371, 2026-03-08; not copied |
| gemini-2533-prompt | gemini:2533 response → gemini:2533 prompt | Gemini web (lineage), thread th_6903e371, 2026-03-08; not copied |
| gemini-2534-prompt | gemini:2534 response → gemini:2534 prompt | Gemini web (lineage), thread th_6903e371, 2026-03-08; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2535-prompt | gemini:2535 response → gemini:2535 prompt | Gemini web (lineage), thread th_6903e371, 2026-03-08; copied: 2 pasted, 0 lifted into archive notes |
| gemini-2536-prompt | gemini:2536 response → gemini:2536 prompt | Gemini web (lineage), thread th_6903e371, 2026-03-08; not copied |
| gemini-2537-prompt | gemini:2537 response → gemini:2537 prompt | Gemini web (lineage), thread th_6903e371, 2026-03-08; copied: 1 pasted, 1 lifted into archive notes |
| gemini-2538-prompt | gemini:2538 response → gemini:2538 prompt | Gemini web (lineage), thread th_6903e371, 2026-03-08; not copied |
| gemini-2539-prompt | gemini:2539 response → gemini:2539 prompt | Gemini web (lineage), thread th_6903e371, 2026-03-08; not copied |
| gemini-2540-prompt | gemini:2540 response → gemini:2540 prompt | Gemini web (lineage), thread th_6903e371, 2026-03-08; copied: 3 pasted, 0 lifted into archive notes |
| gemini-2541-prompt | gemini:2541 response → gemini:2541 prompt | Gemini web (lineage), thread th_6903e371, 2026-03-08; not copied |
| gemini-2542-prompt | gemini:2542 response → gemini:2542 prompt | Gemini web (lineage), thread th_6903e371, 2026-03-08; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2202-prompt | gemini:2202 response → gemini:2202 prompt | Gemini web (lineage), thread th_6a0712d4, 2026-02-26; not copied |
| gemini-3018-prompt | gemini:3018 response → gemini:3018 prompt | Gemini web (lineage), thread th_6ab527fd, 2026-03-28; not copied |
| gemini-3019-prompt | gemini:3019 response → gemini:3019 prompt | Gemini web (lineage), thread th_6ab527fd, 2026-03-28; not copied |
| gemini-2247-prompt | gemini:2247 response → gemini:2247 prompt | Gemini web (lineage), thread th_6bb998ae, 2026-02-28; not copied |
| gemini-2248-prompt | gemini:2248 response → gemini:2248 prompt | Gemini web (lineage), thread th_6bb998ae, 2026-02-28; not copied |
| gemini-2249-prompt | gemini:2249 response → gemini:2249 prompt | Gemini web (lineage), thread th_6bb998ae, 2026-02-28; not copied |
| gemini-2250-prompt | gemini:2250 response → gemini:2250 prompt | Gemini web (lineage), thread th_6bb998ae, 2026-02-28; not copied |
| gemini-2251-prompt | gemini:2251 response → gemini:2251 prompt | Gemini web (lineage), thread th_6bb998ae, 2026-02-28; not copied |
| gemini-2252-prompt | gemini:2252 response → gemini:2252 prompt | Gemini web (lineage), thread th_6bb998ae, 2026-02-28; not copied |
| gemini-2253-prompt | gemini:2253 response → gemini:2253 prompt | Gemini web (lineage), thread th_6bb998ae, 2026-02-28; not copied |
| gemini-2734-prompt | gemini:2734 response → gemini:2734 prompt | Gemini web (lineage), thread th_6bc3da60, 2026-03-17; not copied |
| gemini-2735-prompt | gemini:2735 response → gemini:2735 prompt | Gemini web (lineage), thread th_6bc3da60, 2026-03-17; not copied |
| gemini-2736-prompt | gemini:2736 response → gemini:2736 prompt | Gemini web (lineage), thread th_6bc3da60, 2026-03-17; not copied |
| gemini-2737-prompt | gemini:2737 response → gemini:2737 prompt | Gemini web (lineage), thread th_6bc3da60, 2026-03-17; not copied |
| gemini-2738-prompt | gemini:2738 response → gemini:2738 prompt | Gemini web (lineage), thread th_6bc3da60, 2026-03-17; not copied |
| gemini-2739-prompt | gemini:2739 response → gemini:2739 prompt | Gemini web (lineage), thread th_6bc3da60, 2026-03-17; not copied |
| gemini-2740-prompt | gemini:2740 response → gemini:2740 prompt | Gemini web (lineage), thread th_6bc3da60, 2026-03-17; not copied |
| gemini-2741-prompt | gemini:2741 response → gemini:2741 prompt | Gemini web (lineage), thread th_6bc3da60, 2026-03-17; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2742-prompt | gemini:2742 response → gemini:2742 prompt | Gemini web (lineage), thread th_6bc3da60, 2026-03-17; not copied |
| gemini-2743-prompt | gemini:2743 response → gemini:2743 prompt | Gemini web (lineage), thread th_6bc3da60, 2026-03-17; not copied |
| gemini-3012-prompt | gemini:3012 response → gemini:3012 prompt | Gemini web (lineage), thread th_6bf68d8c, 2026-03-28; not copied |
| gemini-3013-prompt | gemini:3013 response → gemini:3013 prompt | Gemini web (lineage), thread th_6bf68d8c, 2026-03-28; not copied |
| gemini-3014-prompt | gemini:3014 response → gemini:3014 prompt | Gemini web (lineage), thread th_6bf68d8c, 2026-03-28; not copied |
| gemini-3015-prompt | gemini:3015 response → gemini:3015 prompt | Gemini web (lineage), thread th_6bf68d8c, 2026-03-28; not copied |
| gemini-3016-prompt | gemini:3016 response → gemini:3016 prompt | Gemini web (lineage), thread th_6bf68d8c, 2026-03-28; not copied |
| gemini-3017-prompt | gemini:3017 response → gemini:3017 prompt | Gemini web (lineage), thread th_6bf68d8c, 2026-03-28; not copied |
| gemini-190-prompt | gemini:190 response → gemini:190 prompt | Gemini web (lineage), thread th_6bfc791a, 2025-12-04; not copied |
| gemini-191-prompt | gemini:191 response → gemini:191 prompt | Gemini web (lineage), thread th_6bfc791a, 2025-12-04; copied: 1 pasted, 0 lifted into archive notes |
| gemini-911-prompt | gemini:911 response → gemini:911 prompt | Gemini web (lineage), thread th_6cd40521, 2026-01-22; not copied |
| gemini-889-prompt | gemini:889 response → gemini:889 prompt | Gemini web (lineage), thread th_6cd5e7c4, 2026-01-22; not copied |
| gemini-322-prompt | gemini:322 response → gemini:322 prompt | Gemini web (lineage), thread th_6d59f3d7, 2025-12-08; copied: 4 pasted, 0 lifted into archive notes |
| gemini-1858-prompt | gemini:1858 response → gemini:1858 prompt | Gemini web (lineage), thread th_6e5aba60, 2026-02-17; not copied |
| gemini-859-prompt | gemini:859 response → gemini:859 prompt | Gemini web (lineage), thread th_6e77d8f0, 2026-01-19; not copied |
| gemini-860-prompt | gemini:860 response → gemini:860 prompt | Gemini web (lineage), thread th_6e77d8f0, 2026-01-19; not copied |
| gemini-861-prompt | gemini:861 response → gemini:861 prompt | Gemini web (lineage), thread th_6e77d8f0, 2026-01-19; not copied |
| gemini-862-prompt | gemini:862 response → gemini:862 prompt | Gemini web (lineage), thread th_6e77d8f0, 2026-01-19; not copied |
| gemini-2098-prompt | gemini:2098 response → gemini:2098 prompt | Gemini web (lineage), thread th_6eaa3668, 2026-02-25; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2099-prompt | gemini:2099 response → gemini:2099 prompt | Gemini web (lineage), thread th_6eaa3668, 2026-02-25; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2100-prompt | gemini:2100 response → gemini:2100 prompt | Gemini web (lineage), thread th_6eaa3668, 2026-02-25; not copied |
| gemini-2877-prompt | gemini:2877 response → gemini:2877 prompt | Gemini web (lineage), thread th_6edafa7f, 2026-03-23; copied: 1 pasted, 0 lifted into archive notes |
| gemini-1446-prompt | gemini:1446 response → gemini:1446 prompt | Gemini web (lineage), thread th_6eff1270, 2026-02-07; copied: 2 pasted, 0 lifted into archive notes |
| gemini-858-prompt | gemini:858 response → gemini:858 prompt | Gemini web (lineage), thread th_7009fee5, 2026-01-19; not copied |
| gemini-1834-prompt | gemini:1834 response → gemini:1834 prompt | Gemini web (lineage), thread th_70b8b5eb, 2026-02-13; not copied |
| gemini-1396-prompt | gemini:1396 response → gemini:1396 prompt | Gemini web (lineage), thread th_70c23289, 2026-02-06; copied: 3 pasted, 1 lifted into archive notes |
| gemini-1397-prompt | gemini:1397 response → gemini:1397 prompt | Gemini web (lineage), thread th_70c23289, 2026-02-06; copied: 2 pasted, 0 lifted into archive notes |
| gemini-2492-prompt | gemini:2492 response → gemini:2492 prompt | Gemini web (lineage), thread th_7122ed31, 2026-03-06; not copied |
| gemini-2493-prompt | gemini:2493 response → gemini:2493 prompt | Gemini web (lineage), thread th_7122ed31, 2026-03-06; copied: 4 pasted, 0 lifted into archive notes |
| gemini-2494-prompt | gemini:2494 response → gemini:2494 prompt | Gemini web (lineage), thread th_7122ed31, 2026-03-06; not copied |
| gemini-2495-prompt | gemini:2495 response → gemini:2495 prompt | Gemini web (lineage), thread th_7122ed31, 2026-03-06; not copied |
| gemini-2496-prompt | gemini:2496 response → gemini:2496 prompt | Gemini web (lineage), thread th_7122ed31, 2026-03-06; not copied |
| gemini-1094-prompt | gemini:1094 response → gemini:1094 prompt | Gemini web (lineage), thread th_712cf111, 2026-01-26; not copied |
| gemini-944-prompt | gemini:944 response → gemini:944 prompt | Gemini web (lineage), thread th_7131ab2e, 2026-01-22; not copied |
| gemini-945-prompt | gemini:945 response → gemini:945 prompt | Gemini web (lineage), thread th_7131ab2e, 2026-01-22; not copied |
| gemini-1039-prompt | gemini:1039 response → gemini:1039 prompt | Gemini web (lineage), thread th_7143555b, 2026-01-25; not copied |
| gemini-1040-prompt | gemini:1040 response → gemini:1040 prompt | Gemini web (lineage), thread th_7143555b, 2026-01-25; copied: 1 pasted, 0 lifted into archive notes |
| gemini-1041-prompt | gemini:1041 response → gemini:1041 prompt | Gemini web (lineage), thread th_7143555b, 2026-01-25; not copied |
| gemini-1042-prompt | gemini:1042 response → gemini:1042 prompt | Gemini web (lineage), thread th_7143555b, 2026-01-25; not copied |
| gemini-1043-prompt | gemini:1043 response → gemini:1043 prompt | Gemini web (lineage), thread th_7143555b, 2026-01-25; not copied |
| gemini-1044-prompt | gemini:1044 response → gemini:1044 prompt | Gemini web (lineage), thread th_7143555b, 2026-01-25; not copied |
| gemini-1045-prompt | gemini:1045 response → gemini:1045 prompt | Gemini web (lineage), thread th_7143555b, 2026-01-25; not copied |
| gemini-1046-prompt | gemini:1046 response → gemini:1046 prompt | Gemini web (lineage), thread th_7143555b, 2026-01-25; not copied |
| gemini-1047-prompt | gemini:1047 response → gemini:1047 prompt | Gemini web (lineage), thread th_7143555b, 2026-01-25; not copied |
| gemini-1048-prompt | gemini:1048 response → gemini:1048 prompt | Gemini web (lineage), thread th_7143555b, 2026-01-25; not copied |
| gemini-1049-prompt | gemini:1049 response → gemini:1049 prompt | Gemini web (lineage), thread th_7143555b, 2026-01-25; not copied |
| gemini-680-prompt | gemini:680 response → gemini:680 prompt | Gemini web (lineage), thread th_73440098, 2026-01-13; not copied |
| gemini-681-prompt | gemini:681 response → gemini:681 prompt | Gemini web (lineage), thread th_73440098, 2026-01-13; not copied |
| gemini-682-prompt | gemini:682 response → gemini:682 prompt | Gemini web (lineage), thread th_73440098, 2026-01-13; not copied |
| gemini-683-prompt | gemini:683 response → gemini:683 prompt | Gemini web (lineage), thread th_73440098, 2026-01-13; not copied |
| gemini-684-prompt | gemini:684 response → gemini:684 prompt | Gemini web (lineage), thread th_73440098, 2026-01-13; not copied |
| gemini-685-prompt | gemini:685 response → gemini:685 prompt | Gemini web (lineage), thread th_73440098, 2026-01-13; copied: 1 pasted, 0 lifted into archive notes |
| gemini-686-prompt | gemini:686 response → gemini:686 prompt | Gemini web (lineage), thread th_73440098, 2026-01-13; not copied |
| gemini-960-prompt | gemini:960 response → gemini:960 prompt | Gemini web (lineage), thread th_73ecdee4, 2026-01-23; copied: 1 pasted, 0 lifted into archive notes |
| gemini-961-prompt | gemini:961 response → gemini:961 prompt | Gemini web (lineage), thread th_73ecdee4, 2026-01-23; not copied |
| gemini-962-prompt | gemini:962 response → gemini:962 prompt | Gemini web (lineage), thread th_73ecdee4, 2026-01-23; not copied |
| gemini-963-prompt | gemini:963 response → gemini:963 prompt | Gemini web (lineage), thread th_73ecdee4, 2026-01-23; not copied |
| gemini-964-prompt | gemini:964 response → gemini:964 prompt | Gemini web (lineage), thread th_73ecdee4, 2026-01-23; not copied |
| gemini-965-prompt | gemini:965 response → gemini:965 prompt | Gemini web (lineage), thread th_73ecdee4, 2026-01-23; not copied |
| gemini-966-prompt | gemini:966 response → gemini:966 prompt | Gemini web (lineage), thread th_73ecdee4, 2026-01-23; copied: 0 pasted, 2 lifted into archive notes |
| gemini-967-prompt | gemini:967 response → gemini:967 prompt | Gemini web (lineage), thread th_73ecdee4, 2026-01-23; not copied |
| gemini-968-prompt | gemini:968 response → gemini:968 prompt | Gemini web (lineage), thread th_73ecdee4, 2026-01-23; not copied |
| gemini-969-prompt | gemini:969 response → gemini:969 prompt | Gemini web (lineage), thread th_73ecdee4, 2026-01-23; not copied |
| gemini-970-prompt | gemini:970 response → gemini:970 prompt | Gemini web (lineage), thread th_73ecdee4, 2026-01-23; not copied |
| gemini-971-prompt | gemini:971 response → gemini:971 prompt | Gemini web (lineage), thread th_73ecdee4, 2026-01-23; not copied |
| gemini-972-prompt | gemini:972 response → gemini:972 prompt | Gemini web (lineage), thread th_73ecdee4, 2026-01-23; not copied |
| gemini-973-prompt | gemini:973 response → gemini:973 prompt | Gemini web (lineage), thread th_73ecdee4, 2026-01-23; not copied |
| gemini-974-prompt | gemini:974 response → gemini:974 prompt | Gemini web (lineage), thread th_73ecdee4, 2026-01-23; copied: 0 pasted, 1 lifted into archive notes |
| gemini-975-prompt | gemini:975 response → gemini:975 prompt | Gemini web (lineage), thread th_73ecdee4, 2026-01-23; not copied |
| gemini-976-prompt | gemini:976 response → gemini:976 prompt | Gemini web (lineage), thread th_73ecdee4, 2026-01-23; not copied |
| gemini-977-prompt | gemini:977 response → gemini:977 prompt | Gemini web (lineage), thread th_73ecdee4, 2026-01-23; copied: 1 pasted, 1 lifted into archive notes |
| gemini-978-prompt | gemini:978 response → gemini:978 prompt | Gemini web (lineage), thread th_73ecdee4, 2026-01-23; not copied |
| gemini-979-prompt | gemini:979 response → gemini:979 prompt | Gemini web (lineage), thread th_73ecdee4, 2026-01-23; not copied |
| gemini-980-prompt | gemini:980 response → gemini:980 prompt | Gemini web (lineage), thread th_73ecdee4, 2026-01-23; not copied |
| gemini-981-prompt | gemini:981 response → gemini:981 prompt | Gemini web (lineage), thread th_73ecdee4, 2026-01-23; not copied |
| gemini-982-prompt | gemini:982 response → gemini:982 prompt | Gemini web (lineage), thread th_73ecdee4, 2026-01-23; not copied |
| gemini-983-prompt | gemini:983 response → gemini:983 prompt | Gemini web (lineage), thread th_73ecdee4, 2026-01-23; copied: 2 pasted, 1 lifted into archive notes |
| gemini-984-prompt | gemini:984 response → gemini:984 prompt | Gemini web (lineage), thread th_73ecdee4, 2026-01-23; not copied |
| gemini-985-prompt | gemini:985 response → gemini:985 prompt | Gemini web (lineage), thread th_73ecdee4, 2026-01-23; not copied |
| gemini-986-prompt | gemini:986 response → gemini:986 prompt | Gemini web (lineage), thread th_73ecdee4, 2026-01-23; not copied |
| gemini-987-prompt | gemini:987 response → gemini:987 prompt | Gemini web (lineage), thread th_73ecdee4, 2026-01-23; not copied |
| gemini-988-prompt | gemini:988 response → gemini:988 prompt | Gemini web (lineage), thread th_73ecdee4, 2026-01-23; not copied |
| gemini-989-prompt | gemini:989 response → gemini:989 prompt | Gemini web (lineage), thread th_73ecdee4, 2026-01-23; not copied |
| gemini-990-prompt | gemini:990 response → gemini:990 prompt | Gemini web (lineage), thread th_73ecdee4, 2026-01-23; not copied |
| gemini-991-prompt | gemini:991 response → gemini:991 prompt | Gemini web (lineage), thread th_73ecdee4, 2026-01-23; copied: 2 pasted, 0 lifted into archive notes |
| gemini-992-prompt | gemini:992 response → gemini:992 prompt | Gemini web (lineage), thread th_73ecdee4, 2026-01-23; copied: 1 pasted, 0 lifted into archive notes |
| gemini-993-prompt | gemini:993 response → gemini:993 prompt | Gemini web (lineage), thread th_73ecdee4, 2026-01-23; not copied |
| gemini-452-prompt | gemini:452 response → gemini:452 prompt | Gemini web (lineage), thread th_74404e3b, 2026-01-07; not copied |
| gemini-634-prompt | gemini:634 response → gemini:634 prompt | Gemini web (lineage), thread th_7501296d, 2026-01-11; copied: 1 pasted, 0 lifted into archive notes |
| gemini-635-prompt | gemini:635 response → gemini:635 prompt | Gemini web (lineage), thread th_7501296d, 2026-01-11; not copied |
| gemini-636-prompt | gemini:636 response → gemini:636 prompt | Gemini web (lineage), thread th_7501296d, 2026-01-11; not copied |
| gemini-2658-prompt | gemini:2658 response → gemini:2658 prompt | Gemini web (lineage), thread th_7581b7ec, 2026-03-10; not copied |
| gemini-2659-prompt | gemini:2659 response → gemini:2659 prompt | Gemini web (lineage), thread th_7581b7ec, 2026-03-10; not copied |
| gemini-857-prompt | gemini:857 response → gemini:857 prompt | Gemini web (lineage), thread th_75a1fe12, 2026-01-19; not copied |
| gemini-90-prompt | gemini:90 response → gemini:90 prompt | Gemini web (lineage), thread th_75e4e78e, 2025-12-02; copied: 2 pasted, 0 lifted into archive notes |
| gemini-91-prompt | gemini:91 response → gemini:91 prompt | Gemini web (lineage), thread th_75e4e78e, 2025-12-02; not copied |
| gemini-1900-prompt | gemini:1900 response → gemini:1900 prompt | Gemini web (lineage), thread th_7634f793, 2026-02-18; not copied |
| gemini-131-prompt | gemini:131 response → gemini:131 prompt | Gemini web (lineage), thread th_76686609, 2025-12-03; not copied |
| gemini-132-prompt | gemini:132 response → gemini:132 prompt | Gemini web (lineage), thread th_76686609, 2025-12-03; copied: 1 pasted, 0 lifted into archive notes |
| gemini-1802-prompt | gemini:1802 response → gemini:1802 prompt | Gemini web (lineage), thread th_76ed4190, 2026-02-13; not copied |
| gemini-1803-prompt | gemini:1803 response → gemini:1803 prompt | Gemini web (lineage), thread th_76ed4190, 2026-02-13; not copied |
| gemini-1804-prompt | gemini:1804 response → gemini:1804 prompt | Gemini web (lineage), thread th_76ed4190, 2026-02-13; not copied |
| gemini-1805-prompt | gemini:1805 response → gemini:1805 prompt | Gemini web (lineage), thread th_76ed4190, 2026-02-13; not copied |
| gemini-1806-prompt | gemini:1806 response → gemini:1806 prompt | Gemini web (lineage), thread th_76ed4190, 2026-02-13; not copied |
| gemini-1807-prompt | gemini:1807 response → gemini:1807 prompt | Gemini web (lineage), thread th_76ed4190, 2026-02-13; not copied |
| gemini-1808-prompt | gemini:1808 response → gemini:1808 prompt | Gemini web (lineage), thread th_76ed4190, 2026-02-13; not copied |
| gemini-1809-prompt | gemini:1809 response → gemini:1809 prompt | Gemini web (lineage), thread th_76ed4190, 2026-02-13; not copied |
| gemini-1810-prompt | gemini:1810 response → gemini:1810 prompt | Gemini web (lineage), thread th_76ed4190, 2026-02-13; not copied |
| gemini-1811-prompt | gemini:1811 response → gemini:1811 prompt | Gemini web (lineage), thread th_76ed4190, 2026-02-13; not copied |
| gemini-1812-prompt | gemini:1812 response → gemini:1812 prompt | Gemini web (lineage), thread th_76ed4190, 2026-02-13; not copied |
| gemini-1813-prompt | gemini:1813 response → gemini:1813 prompt | Gemini web (lineage), thread th_76ed4190, 2026-02-13; not copied |
| gemini-1814-prompt | gemini:1814 response → gemini:1814 prompt | Gemini web (lineage), thread th_76ed4190, 2026-02-13; not copied |
| gemini-1731-prompt | gemini:1731 response → gemini:1731 prompt | Gemini web (lineage), thread th_773dda52, 2026-02-11; not copied |
| gemini-1732-prompt | gemini:1732 response → gemini:1732 prompt | Gemini web (lineage), thread th_773dda52, 2026-02-11; not copied |
| gemini-1733-prompt | gemini:1733 response → gemini:1733 prompt | Gemini web (lineage), thread th_773dda52, 2026-02-11; copied: 1 pasted, 0 lifted into archive notes |
| gemini-1734-prompt | gemini:1734 response → gemini:1734 prompt | Gemini web (lineage), thread th_773dda52, 2026-02-11; not copied |
| gemini-1735-prompt | gemini:1735 response → gemini:1735 prompt | Gemini web (lineage), thread th_773dda52, 2026-02-11; not copied |
| gemini-1736-prompt | gemini:1736 response → gemini:1736 prompt | Gemini web (lineage), thread th_773dda52, 2026-02-11; not copied |
| gemini-1544-prompt | gemini:1544 response → gemini:1544 prompt | Gemini web (lineage), thread th_77b6d6f1, 2026-02-09; not copied |
| gemini-1545-prompt | gemini:1545 response → gemini:1545 prompt | Gemini web (lineage), thread th_77b6d6f1, 2026-02-09; not copied |
| gemini-1546-prompt | gemini:1546 response → gemini:1546 prompt | Gemini web (lineage), thread th_77b6d6f1, 2026-02-09; not copied |
| gemini-935-prompt | gemini:935 response → gemini:935 prompt | Gemini web (lineage), thread th_7856db29, 2026-01-22; not copied |
| gemini-936-prompt | gemini:936 response → gemini:936 prompt | Gemini web (lineage), thread th_7856db29, 2026-01-22; not copied |
| gemini-937-prompt | gemini:937 response → gemini:937 prompt | Gemini web (lineage), thread th_7856db29, 2026-01-22; copied: 1 pasted, 0 lifted into archive notes |
| gemini-938-prompt | gemini:938 response → gemini:938 prompt | Gemini web (lineage), thread th_7856db29, 2026-01-22; not copied |
| gemini-939-prompt | gemini:939 response → gemini:939 prompt | Gemini web (lineage), thread th_7856db29, 2026-01-22; not copied |
| gemini-940-prompt | gemini:940 response → gemini:940 prompt | Gemini web (lineage), thread th_7856db29, 2026-01-22; not copied |
| gemini-123-prompt | gemini:123 response → gemini:123 prompt | Gemini web (lineage), thread th_78b8c9ee, 2025-12-03; not copied |
| gemini-124-prompt | gemini:124 response → gemini:124 prompt | Gemini web (lineage), thread th_78b8c9ee, 2025-12-03; not copied |
| gemini-125-prompt | gemini:125 response → gemini:125 prompt | Gemini web (lineage), thread th_78b8c9ee, 2025-12-03; not copied |
| gemini-126-prompt | gemini:126 response → gemini:126 prompt | Gemini web (lineage), thread th_78b8c9ee, 2025-12-03; copied: 1 pasted, 0 lifted into archive notes |
| gemini-1313-prompt | gemini:1313 response → gemini:1313 prompt | Gemini web (lineage), thread th_7943b312, 2026-02-03; not copied |
| gemini-1314-prompt | gemini:1314 response → gemini:1314 prompt | Gemini web (lineage), thread th_7943b312, 2026-02-03; not copied |
| gemini-1315-prompt | gemini:1315 response → gemini:1315 prompt | Gemini web (lineage), thread th_7943b312, 2026-02-03; not copied |
| gemini-2634-prompt | gemini:2634 response → gemini:2634 prompt | Gemini web (lineage), thread th_7959db32, 2026-03-10; not copied |
| gemini-2635-prompt | gemini:2635 response → gemini:2635 prompt | Gemini web (lineage), thread th_7959db32, 2026-03-10; copied: 2 pasted, 0 lifted into archive notes |
| gemini-225-prompt | gemini:225 response → gemini:225 prompt | Gemini web (lineage), thread th_795f55b9, 2025-12-05; not copied |
| gemini-1542-prompt | gemini:1542 response → gemini:1542 prompt | Gemini web (lineage), thread th_79685d65, 2026-02-09; not copied |
| gemini-1543-prompt | gemini:1543 response → gemini:1543 prompt | Gemini web (lineage), thread th_79685d65, 2026-02-09; not copied |
| gemini-2358-prompt | gemini:2358 response → gemini:2358 prompt | Gemini web (lineage), thread th_79b4173a, 2026-03-04; copied: 2 pasted, 0 lifted into archive notes |
| gemini-2359-prompt | gemini:2359 response → gemini:2359 prompt | Gemini web (lineage), thread th_79b4173a, 2026-03-04; not copied |
| gemini-2360-prompt | gemini:2360 response → gemini:2360 prompt | Gemini web (lineage), thread th_79b4173a, 2026-03-04; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2361-prompt | gemini:2361 response → gemini:2361 prompt | Gemini web (lineage), thread th_79b4173a, 2026-03-04; not copied |
| gemini-2362-prompt | gemini:2362 response → gemini:2362 prompt | Gemini web (lineage), thread th_79b4173a, 2026-03-04; not copied |
| gemini-2363-prompt | gemini:2363 response → gemini:2363 prompt | Gemini web (lineage), thread th_79b4173a, 2026-03-04; copied: 0 pasted, 1 lifted into archive notes |
| gemini-2364-prompt | gemini:2364 response → gemini:2364 prompt | Gemini web (lineage), thread th_79b4173a, 2026-03-04; not copied |
| gemini-2365-prompt | gemini:2365 response → gemini:2365 prompt | Gemini web (lineage), thread th_79b4173a, 2026-03-04; copied: 2 pasted, 0 lifted into archive notes |
| gemini-2366-prompt | gemini:2366 response → gemini:2366 prompt | Gemini web (lineage), thread th_79b4173a, 2026-03-04; not copied |
| gemini-2367-prompt | gemini:2367 response → gemini:2367 prompt | Gemini web (lineage), thread th_79b4173a, 2026-03-04; copied: 2 pasted, 0 lifted into archive notes |
| gemini-2368-prompt | gemini:2368 response → gemini:2368 prompt | Gemini web (lineage), thread th_79b4173a, 2026-03-04; copied: 2 pasted, 0 lifted into archive notes |
| gemini-2369-prompt | gemini:2369 response → gemini:2369 prompt | Gemini web (lineage), thread th_79b4173a, 2026-03-04; copied: 1 pasted, 0 lifted into archive notes |
| gemini-583-prompt | gemini:583 response → gemini:583 prompt | Gemini web (lineage), thread th_79d66a3d, 2026-01-10; not copied |
| gemini-2420-prompt | gemini:2420 response → gemini:2420 prompt | Gemini web (lineage), thread th_7a77fafb, 2026-03-05; not copied |
| gemini-2421-prompt | gemini:2421 response → gemini:2421 prompt | Gemini web (lineage), thread th_7a77fafb, 2026-03-05; not copied |
| gemini-2422-prompt | gemini:2422 response → gemini:2422 prompt | Gemini web (lineage), thread th_7a77fafb, 2026-03-05; not copied |
| gemini-2423-prompt | gemini:2423 response → gemini:2423 prompt | Gemini web (lineage), thread th_7a77fafb, 2026-03-05; not copied |
| gemini-946-prompt | gemini:946 response → gemini:946 prompt | Gemini web (lineage), thread th_7ad9562f, 2026-01-22 to 2026-01-23; not copied |
| gemini-947-prompt | gemini:947 response → gemini:947 prompt | Gemini web (lineage), thread th_7ad9562f, 2026-01-22 to 2026-01-23; not copied |
| gemini-948-prompt | gemini:948 response → gemini:948 prompt | Gemini web (lineage), thread th_7ad9562f, 2026-01-22 to 2026-01-23; not copied |
| gemini-252-prompt | gemini:252 response → gemini:252 prompt | Gemini web (lineage), thread th_7bbd0c8e, 2025-12-06; not copied |
| gemini-481-prompt | gemini:481 response → gemini:481 prompt | Gemini web (lineage), thread th_7c96b1f7, 2026-01-08; not copied |
| gemini-482-prompt | gemini:482 response → gemini:482 prompt | Gemini web (lineage), thread th_7c96b1f7, 2026-01-08; not copied |
| gemini-483-prompt | gemini:483 response → gemini:483 prompt | Gemini web (lineage), thread th_7c96b1f7, 2026-01-08; not copied |
| gemini-608-prompt | gemini:608 response → gemini:608 prompt | Gemini web (lineage), thread th_7d4f6c3a, 2026-01-10; not copied |
| gemini-609-prompt | gemini:609 response → gemini:609 prompt | Gemini web (lineage), thread th_7d4f6c3a, 2026-01-10; not copied |
| gemini-610-prompt | gemini:610 response → gemini:610 prompt | Gemini web (lineage), thread th_7d4f6c3a, 2026-01-10; not copied |
| gemini-611-prompt | gemini:611 response → gemini:611 prompt | Gemini web (lineage), thread th_7d4f6c3a, 2026-01-10; not copied |
| gemini-612-prompt | gemini:612 response → gemini:612 prompt | Gemini web (lineage), thread th_7d4f6c3a, 2026-01-10; not copied |
| gemini-613-prompt | gemini:613 response → gemini:613 prompt | Gemini web (lineage), thread th_7d4f6c3a, 2026-01-10; not copied |
| gemini-2663-prompt | gemini:2663 response → gemini:2663 prompt | Gemini web (lineage), thread th_7db5d0cf, 2026-03-11; not copied |
| gemini-2664-prompt | gemini:2664 response → gemini:2664 prompt | Gemini web (lineage), thread th_7db5d0cf, 2026-03-11; not copied |
| gemini-2379-prompt | gemini:2379 response → gemini:2379 prompt | Gemini web (lineage), thread th_7dd1a901, 2026-03-04; copied: 4 pasted, 0 lifted into archive notes |
| gemini-906-prompt | gemini:906 response → gemini:906 prompt | Gemini web (lineage), thread th_7deed7e1, 2026-01-22; not copied |
| gemini-907-prompt | gemini:907 response → gemini:907 prompt | Gemini web (lineage), thread th_7deed7e1, 2026-01-22; not copied |
| gemini-1975-prompt | gemini:1975 response → gemini:1975 prompt | Gemini web (lineage), thread th_7e97e2ed, 2026-02-20; not copied |
| gemini-2453-prompt | gemini:2453 response → gemini:2453 prompt | Gemini web (lineage), thread th_7f0d653c, 2026-03-06; not copied |
| gemini-2454-prompt | gemini:2454 response → gemini:2454 prompt | Gemini web (lineage), thread th_7f0d653c, 2026-03-06; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2455-prompt | gemini:2455 response → gemini:2455 prompt | Gemini web (lineage), thread th_7f0d653c, 2026-03-06; not copied |
| gemini-2456-prompt | gemini:2456 response → gemini:2456 prompt | Gemini web (lineage), thread th_7f0d653c, 2026-03-06; not copied |
| gemini-2457-prompt | gemini:2457 response → gemini:2457 prompt | Gemini web (lineage), thread th_7f0d653c, 2026-03-06; not copied |
| gemini-2458-prompt | gemini:2458 response → gemini:2458 prompt | Gemini web (lineage), thread th_7f0d653c, 2026-03-06; not copied |
| gemini-2459-prompt | gemini:2459 response → gemini:2459 prompt | Gemini web (lineage), thread th_7f0d653c, 2026-03-06; not copied |
| gemini-994-prompt | gemini:994 response → gemini:994 prompt | Gemini web (lineage), thread th_7f39f1bd, 2026-01-23; not copied |
| gemini-2703-prompt | gemini:2703 response → gemini:2703 prompt | Gemini web (lineage), thread th_7fbd4e6b, 2026-03-16; not copied |
| gemini-2704-prompt | gemini:2704 response → gemini:2704 prompt | Gemini web (lineage), thread th_7fbd4e6b, 2026-03-16; not copied |
| gemini-1148-prompt | gemini:1148 response → gemini:1148 prompt | Gemini web (lineage), thread th_802d0824, 2026-01-27; copied: 0 pasted, 1 lifted into archive notes |
| gemini-1149-prompt | gemini:1149 response → gemini:1149 prompt | Gemini web (lineage), thread th_802d0824, 2026-01-27; not copied |
| gemini-1150-prompt | gemini:1150 response → gemini:1150 prompt | Gemini web (lineage), thread th_802d0824, 2026-01-27; not copied |
| gemini-1151-prompt | gemini:1151 response → gemini:1151 prompt | Gemini web (lineage), thread th_802d0824, 2026-01-27; not copied |
| gemini-1152-prompt | gemini:1152 response → gemini:1152 prompt | Gemini web (lineage), thread th_802d0824, 2026-01-27; copied: 3 pasted, 0 lifted into archive notes |
| gemini-1153-prompt | gemini:1153 response → gemini:1153 prompt | Gemini web (lineage), thread th_802d0824, 2026-01-27; not copied |
| gemini-1154-prompt | gemini:1154 response → gemini:1154 prompt | Gemini web (lineage), thread th_802d0824, 2026-01-27; not copied |
| gemini-1155-prompt | gemini:1155 response → gemini:1155 prompt | Gemini web (lineage), thread th_802d0824, 2026-01-27; not copied |
| gemini-1156-prompt | gemini:1156 response → gemini:1156 prompt | Gemini web (lineage), thread th_802d0824, 2026-01-27; not copied |
| gemini-2618-prompt | gemini:2618 response → gemini:2618 prompt | Gemini web (lineage), thread th_805c85c0, 2026-03-10; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2619-prompt | gemini:2619 response → gemini:2619 prompt | Gemini web (lineage), thread th_805c85c0, 2026-03-10; not copied |
| gemini-876-prompt | gemini:876 response → gemini:876 prompt | Gemini web (lineage), thread th_81039628, 2026-01-20; copied: 0 pasted, 1 lifted into archive notes |
| gemini-877-prompt | gemini:877 response → gemini:877 prompt | Gemini web (lineage), thread th_81039628, 2026-01-20; not copied |
| gemini-878-prompt | gemini:878 response → gemini:878 prompt | Gemini web (lineage), thread th_81039628, 2026-01-20; not copied |
| gemini-880-prompt | gemini:880 response → gemini:880 prompt | Gemini web (lineage), thread th_81039628, 2026-01-20; not copied |
| gemini-881-prompt | gemini:881 response → gemini:881 prompt | Gemini web (lineage), thread th_81039628, 2026-01-20; not copied |
| gemini-882-prompt | gemini:882 response → gemini:882 prompt | Gemini web (lineage), thread th_81039628, 2026-01-20; not copied |
| gemini-883-prompt | gemini:883 response → gemini:883 prompt | Gemini web (lineage), thread th_81039628, 2026-01-20; not copied |
| gemini-884-prompt | gemini:884 response → gemini:884 prompt | Gemini web (lineage), thread th_81039628, 2026-01-20; not copied |
| gemini-885-prompt | gemini:885 response → gemini:885 prompt | Gemini web (lineage), thread th_81039628, 2026-01-20; not copied |
| gemini-886-prompt | gemini:886 response → gemini:886 prompt | Gemini web (lineage), thread th_81039628, 2026-01-20; not copied |
| gemini-3174-prompt | gemini:3174 response → gemini:3174 prompt | Gemini web (lineage), thread th_813b223a, 2026-04-15; not copied |
| gemini-3175-prompt | gemini:3175 response → gemini:3175 prompt | Gemini web (lineage), thread th_813b223a, 2026-04-15; not copied |
| gemini-3176-prompt | gemini:3176 response → gemini:3176 prompt | Gemini web (lineage), thread th_813b223a, 2026-04-15; not copied |
| gemini-3177-prompt | gemini:3177 response → gemini:3177 prompt | Gemini web (lineage), thread th_813b223a, 2026-04-15; not copied |
| gemini-3178-prompt | gemini:3178 response → gemini:3178 prompt | Gemini web (lineage), thread th_813b223a, 2026-04-15; not copied |
| gemini-3179-prompt | gemini:3179 response → gemini:3179 prompt | Gemini web (lineage), thread th_813b223a, 2026-04-15; not copied |
| gemini-3180-prompt | gemini:3180 response → gemini:3180 prompt | Gemini web (lineage), thread th_813b223a, 2026-04-15; not copied |
| gemini-3181-prompt | gemini:3181 response → gemini:3181 prompt | Gemini web (lineage), thread th_813b223a, 2026-04-15; not copied |
| gemini-3182-prompt | gemini:3182 response → gemini:3182 prompt | Gemini web (lineage), thread th_813b223a, 2026-04-15; not copied |
| gemini-3183-prompt | gemini:3183 response → gemini:3183 prompt | Gemini web (lineage), thread th_813b223a, 2026-04-15; not copied |
| gemini-3184-prompt | gemini:3184 response → gemini:3184 prompt | Gemini web (lineage), thread th_813b223a, 2026-04-15; not copied |
| gemini-3185-prompt | gemini:3185 response → gemini:3185 prompt | Gemini web (lineage), thread th_813b223a, 2026-04-15; not copied |
| gemini-3186-prompt | gemini:3186 response → gemini:3186 prompt | Gemini web (lineage), thread th_813b223a, 2026-04-15; not copied |
| gemini-3187-prompt | gemini:3187 response → gemini:3187 prompt | Gemini web (lineage), thread th_813b223a, 2026-04-15; not copied |
| gemini-3188-prompt | gemini:3188 response → gemini:3188 prompt | Gemini web (lineage), thread th_813b223a, 2026-04-15; not copied |
| gemini-3189-prompt | gemini:3189 response → gemini:3189 prompt | Gemini web (lineage), thread th_813b223a, 2026-04-15; not copied |
| gemini-3190-prompt | gemini:3190 response → gemini:3190 prompt | Gemini web (lineage), thread th_813b223a, 2026-04-15; not copied |
| gemini-3191-prompt | gemini:3191 response → gemini:3191 prompt | Gemini web (lineage), thread th_813b223a, 2026-04-15; not copied |
| gemini-3192-prompt | gemini:3192 response → gemini:3192 prompt | Gemini web (lineage), thread th_813b223a, 2026-04-15; not copied |
| gemini-3193-prompt | gemini:3193 response → gemini:3193 prompt | Gemini web (lineage), thread th_813b223a, 2026-04-15; not copied |
| gemini-3194-prompt | gemini:3194 response → gemini:3194 prompt | Gemini web (lineage), thread th_813b223a, 2026-04-15; not copied |
| gemini-3195-prompt | gemini:3195 response → gemini:3195 prompt | Gemini web (lineage), thread th_813b223a, 2026-04-15; not copied |
| gemini-3196-prompt | gemini:3196 response → gemini:3196 prompt | Gemini web (lineage), thread th_813b223a, 2026-04-15; not copied |
| gemini-1575-prompt | gemini:1575 response → gemini:1575 prompt | Gemini web (lineage), thread th_81675d92, 2026-02-09; not copied |
| gemini-1455-prompt | gemini:1455 response → gemini:1455 prompt | Gemini web (lineage), thread th_81aaf188, 2026-02-07; not copied |
| gemini-1456-prompt | gemini:1456 response → gemini:1456 prompt | Gemini web (lineage), thread th_81aaf188, 2026-02-07; not copied |
| gemini-1457-prompt | gemini:1457 response → gemini:1457 prompt | Gemini web (lineage), thread th_81aaf188, 2026-02-07; copied: 0 pasted, 1 lifted into archive notes |
| gemini-1458-prompt | gemini:1458 response → gemini:1458 prompt | Gemini web (lineage), thread th_81aaf188, 2026-02-07; copied: 1 pasted, 0 lifted into archive notes |
| gemini-1459-prompt | gemini:1459 response → gemini:1459 prompt | Gemini web (lineage), thread th_81aaf188, 2026-02-07; not copied |
| gemini-1460-prompt | gemini:1460 response → gemini:1460 prompt | Gemini web (lineage), thread th_81aaf188, 2026-02-07; not copied |
| gemini-1461-prompt | gemini:1461 response → gemini:1461 prompt | Gemini web (lineage), thread th_81aaf188, 2026-02-07; not copied |
| gemini-1462-prompt | gemini:1462 response → gemini:1462 prompt | Gemini web (lineage), thread th_81aaf188, 2026-02-07; not copied |
| gemini-1226-prompt | gemini:1226 response → gemini:1226 prompt | Gemini web (lineage), thread th_81e64a82, 2026-01-29; not copied |
| gemini-1227-prompt | gemini:1227 response → gemini:1227 prompt | Gemini web (lineage), thread th_81e64a82, 2026-01-29; not copied |
| gemini-1228-prompt | gemini:1228 response → gemini:1228 prompt | Gemini web (lineage), thread th_81e64a82, 2026-01-29; not copied |
| gemini-2782-prompt | gemini:2782 response → gemini:2782 prompt | Gemini web (lineage), thread th_82232ac2, 2026-03-20; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2783-prompt | gemini:2783 response → gemini:2783 prompt | Gemini web (lineage), thread th_82232ac2, 2026-03-20; not copied |
| gemini-2500-prompt | gemini:2500 response → gemini:2500 prompt | Gemini web (lineage), thread th_82a4425e, 2026-03-06; not copied |
| gemini-2501-prompt | gemini:2501 response → gemini:2501 prompt | Gemini web (lineage), thread th_82a4425e, 2026-03-06; not copied |
| gemini-2502-prompt | gemini:2502 response → gemini:2502 prompt | Gemini web (lineage), thread th_82a4425e, 2026-03-06; not copied |
| gemini-2878-prompt | gemini:2878 response → gemini:2878 prompt | Gemini web (lineage), thread th_8320355b, 2026-03-23; not copied |
| gemini-2879-prompt | gemini:2879 response → gemini:2879 prompt | Gemini web (lineage), thread th_8320355b, 2026-03-23; not copied |
| gemini-2880-prompt | gemini:2880 response → gemini:2880 prompt | Gemini web (lineage), thread th_8320355b, 2026-03-23; not copied |
| gemini-2881-prompt | gemini:2881 response → gemini:2881 prompt | Gemini web (lineage), thread th_8320355b, 2026-03-23; not copied |
| gemini-2882-prompt | gemini:2882 response → gemini:2882 prompt | Gemini web (lineage), thread th_8320355b, 2026-03-23; not copied |
| gemini-2883-prompt | gemini:2883 response → gemini:2883 prompt | Gemini web (lineage), thread th_8320355b, 2026-03-23; not copied |
| gemini-2884-prompt | gemini:2884 response → gemini:2884 prompt | Gemini web (lineage), thread th_8320355b, 2026-03-23; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2885-prompt | gemini:2885 response → gemini:2885 prompt | Gemini web (lineage), thread th_8320355b, 2026-03-23; not copied |
| gemini-2886-prompt | gemini:2886 response → gemini:2886 prompt | Gemini web (lineage), thread th_8320355b, 2026-03-23; not copied |
| gemini-2887-prompt | gemini:2887 response → gemini:2887 prompt | Gemini web (lineage), thread th_8320355b, 2026-03-23; not copied |
| gemini-2888-prompt | gemini:2888 response → gemini:2888 prompt | Gemini web (lineage), thread th_8320355b, 2026-03-23; copied: 1 pasted, 0 lifted into archive notes |
| gemini-1198-prompt | gemini:1198 response → gemini:1198 prompt | Gemini web (lineage), thread th_838cfa08, 2026-01-28; not copied |
| gemini-1199-prompt | gemini:1199 response → gemini:1199 prompt | Gemini web (lineage), thread th_838cfa08, 2026-01-28; not copied |
| gemini-1200-prompt | gemini:1200 response → gemini:1200 prompt | Gemini web (lineage), thread th_838cfa08, 2026-01-28; not copied |
| gemini-1201-prompt | gemini:1201 response → gemini:1201 prompt | Gemini web (lineage), thread th_838cfa08, 2026-01-28; not copied |
| gemini-1202-prompt | gemini:1202 response → gemini:1202 prompt | Gemini web (lineage), thread th_838cfa08, 2026-01-28; not copied |
| gemini-1203-prompt | gemini:1203 response → gemini:1203 prompt | Gemini web (lineage), thread th_838cfa08, 2026-01-28; not copied |
| gemini-823-prompt | gemini:823 response → gemini:823 prompt | Gemini web (lineage), thread th_83edec69, 2026-01-18; not copied |
| gemini-2679-prompt | gemini:2679 response → gemini:2679 prompt | Gemini web (lineage), thread th_84352c3d, 2026-03-13; copied: 0 pasted, 1 lifted into archive notes |
| gemini-1225-prompt | gemini:1225 response → gemini:1225 prompt | Gemini web (lineage), thread th_84afbd1e, 2026-01-29; not copied |
| gemini-104-prompt | gemini:104 response → gemini:104 prompt | Gemini web (lineage), thread th_84ca246c, 2025-12-02; copied: 1 pasted, 0 lifted into archive notes |
| gemini-1089-prompt | gemini:1089 response → gemini:1089 prompt | Gemini web (lineage), thread th_85011f02, 2026-01-26; not copied |
| gemini-1090-prompt | gemini:1090 response → gemini:1090 prompt | Gemini web (lineage), thread th_85011f02, 2026-01-26; not copied |
| gemini-1220-prompt | gemini:1220 response → gemini:1220 prompt | Gemini web (lineage), thread th_856bdb91, 2026-01-29; copied: 1 pasted, 0 lifted into archive notes |
| gemini-1221-prompt | gemini:1221 response → gemini:1221 prompt | Gemini web (lineage), thread th_856bdb91, 2026-01-29; copied: 2 pasted, 0 lifted into archive notes |
| gemini-1222-prompt | gemini:1222 response → gemini:1222 prompt | Gemini web (lineage), thread th_856bdb91, 2026-01-29; copied: 1 pasted, 0 lifted into archive notes |
| gemini-1223-prompt | gemini:1223 response → gemini:1223 prompt | Gemini web (lineage), thread th_856bdb91, 2026-01-29; copied: 1 pasted, 0 lifted into archive notes |
| gemini-1224-prompt | gemini:1224 response → gemini:1224 prompt | Gemini web (lineage), thread th_856bdb91, 2026-01-29; not copied |
| gemini-2063-prompt | gemini:2063 response → gemini:2063 prompt | Gemini web (lineage), thread th_8638a5c2, 2026-02-21; not copied |
| gemini-1311-prompt | gemini:1311 response → gemini:1311 prompt | Gemini web (lineage), thread th_863d3933, 2026-02-02; not copied |
| gemini-1312-prompt | gemini:1312 response → gemini:1312 prompt | Gemini web (lineage), thread th_863d3933, 2026-02-02; not copied |
| gemini-1988-prompt | gemini:1988 response → gemini:1988 prompt | Gemini web (lineage), thread th_8642b854, 2026-02-20; not copied |
| gemini-1989-prompt | gemini:1989 response → gemini:1989 prompt | Gemini web (lineage), thread th_8642b854, 2026-02-20; not copied |
| gemini-1990-prompt | gemini:1990 response → gemini:1990 prompt | Gemini web (lineage), thread th_8642b854, 2026-02-20; not copied |
| gemini-1991-prompt | gemini:1991 response → gemini:1991 prompt | Gemini web (lineage), thread th_8642b854, 2026-02-20; not copied |
| gemini-896-prompt | gemini:896 response → gemini:896 prompt | Gemini web (lineage), thread th_8645d147, 2026-01-22; not copied |
| gemini-897-prompt | gemini:897 response → gemini:897 prompt | Gemini web (lineage), thread th_8645d147, 2026-01-22; not copied |
| gemini-898-prompt | gemini:898 response → gemini:898 prompt | Gemini web (lineage), thread th_8645d147, 2026-01-22; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2613-prompt | gemini:2613 response → gemini:2613 prompt | Gemini web (lineage), thread th_86ff3e1d, 2026-03-09 to 2026-03-10; copied: 1 pasted, 1 lifted into archive notes |
| gemini-2614-prompt | gemini:2614 response → gemini:2614 prompt | Gemini web (lineage), thread th_86ff3e1d, 2026-03-09 to 2026-03-10; not copied |
| gemini-2615-prompt | gemini:2615 response → gemini:2615 prompt | Gemini web (lineage), thread th_86ff3e1d, 2026-03-09 to 2026-03-10; not copied |
| gemini-2616-prompt | gemini:2616 response → gemini:2616 prompt | Gemini web (lineage), thread th_86ff3e1d, 2026-03-09 to 2026-03-10; not copied |
| gemini-2617-prompt | gemini:2617 response → gemini:2617 prompt | Gemini web (lineage), thread th_86ff3e1d, 2026-03-09 to 2026-03-10; not copied |
| gemini-223-prompt | gemini:223 response → gemini:223 prompt | Gemini web (lineage), thread th_8701b682, 2025-12-05; copied: 1 pasted, 0 lifted into archive notes |
| gemini-100-prompt | gemini:100 response → gemini:100 prompt | Gemini web (lineage), thread th_87533210, 2025-12-02; not copied |
| gemini-101-prompt | gemini:101 response → gemini:101 prompt | Gemini web (lineage), thread th_87533210, 2025-12-02; not copied |
| gemini-102-prompt | gemini:102 response → gemini:102 prompt | Gemini web (lineage), thread th_87533210, 2025-12-02; not copied |
| gemini-2816-prompt | gemini:2816 response → gemini:2816 prompt | Gemini web (lineage), thread th_888827f3, 2026-03-22; not copied |
| gemini-2817-prompt | gemini:2817 response → gemini:2817 prompt | Gemini web (lineage), thread th_888827f3, 2026-03-22; not copied |
| gemini-2818-prompt | gemini:2818 response → gemini:2818 prompt | Gemini web (lineage), thread th_888827f3, 2026-03-22; not copied |
| gemini-2819-prompt | gemini:2819 response → gemini:2819 prompt | Gemini web (lineage), thread th_888827f3, 2026-03-22; not copied |
| gemini-2820-prompt | gemini:2820 response → gemini:2820 prompt | Gemini web (lineage), thread th_888827f3, 2026-03-22; not copied |
| gemini-2821-prompt | gemini:2821 response → gemini:2821 prompt | Gemini web (lineage), thread th_888827f3, 2026-03-22; not copied |
| gemini-370-prompt | gemini:370 response → gemini:370 prompt | Gemini web (lineage), thread th_88ba621b, 2025-12-28; not copied |
| gemini-371-prompt | gemini:371 response → gemini:371 prompt | Gemini web (lineage), thread th_88ba621b, 2025-12-28; not copied |
| gemini-372-prompt | gemini:372 response → gemini:372 prompt | Gemini web (lineage), thread th_88ba621b, 2025-12-28; not copied |
| gemini-3131-prompt | gemini:3131 response → gemini:3131 prompt | Gemini web (lineage), thread th_88c6aaf0, 2026-04-08; not copied |
| gemini-3132-prompt | gemini:3132 response → gemini:3132 prompt | Gemini web (lineage), thread th_88c6aaf0, 2026-04-08; not copied |
| gemini-3133-prompt | gemini:3133 response → gemini:3133 prompt | Gemini web (lineage), thread th_88c6aaf0, 2026-04-08; not copied |
| gemini-2847-prompt | gemini:2847 response → gemini:2847 prompt | Gemini web (lineage), thread th_892e7021, 2026-03-22; copied: 1 pasted, 0 lifted into archive notes |
| gemini-399-prompt | gemini:399 response → gemini:399 prompt | Gemini web (lineage), thread th_899c6725, 2025-12-28; not copied |
| gemini-865-prompt | gemini:865 response → gemini:865 prompt | Gemini web (lineage), thread th_8a11ec6a, 2026-01-20; not copied |
| gemini-866-prompt | gemini:866 response → gemini:866 prompt | Gemini web (lineage), thread th_8a11ec6a, 2026-01-20; not copied |
| gemini-867-prompt | gemini:867 response → gemini:867 prompt | Gemini web (lineage), thread th_8a11ec6a, 2026-01-20; not copied |
| gemini-868-prompt | gemini:868 response → gemini:868 prompt | Gemini web (lineage), thread th_8a11ec6a, 2026-01-20; not copied |
| gemini-869-prompt | gemini:869 response → gemini:869 prompt | Gemini web (lineage), thread th_8a11ec6a, 2026-01-20; copied: 1 pasted, 1 lifted into archive notes |
| gemini-870-prompt | gemini:870 response → gemini:870 prompt | Gemini web (lineage), thread th_8a11ec6a, 2026-01-20; not copied |
| gemini-2514-prompt | gemini:2514 response → gemini:2514 prompt | Gemini web (lineage), thread th_8a39cf7e, 2026-03-06; not copied |
| gemini-455-prompt | gemini:455 response → gemini:455 prompt | Gemini web (lineage), thread th_8a81c35a, 2026-01-07; not copied |
| gemini-747-prompt | gemini:747 response → gemini:747 prompt | Gemini web (lineage), thread th_8ab0e3a3, 2026-01-14; not copied |
| gemini-2393-prompt | gemini:2393 response → gemini:2393 prompt | Gemini web (lineage), thread th_8ad3fa2c, 2026-03-04; not copied |
| gemini-410-prompt | gemini:410 response → gemini:410 prompt | Gemini web (lineage), thread th_8b850a8c, 2025-12-28; copied: 1 pasted, 0 lifted into archive notes |
| gemini-484-prompt | gemini:484 response → gemini:484 prompt | Gemini web (lineage), thread th_8bcf6d4d, 2026-01-08; not copied |
| gemini-756-prompt | gemini:756 response → gemini:756 prompt | Gemini web (lineage), thread th_8bd009a4, 2026-01-15; not copied |
| gemini-757-prompt | gemini:757 response → gemini:757 prompt | Gemini web (lineage), thread th_8bd009a4, 2026-01-15; not copied |
| gemini-758-prompt | gemini:758 response → gemini:758 prompt | Gemini web (lineage), thread th_8bd009a4, 2026-01-15; not copied |
| gemini-3234-prompt | gemini:3234 response → gemini:3234 prompt | Gemini web (lineage), thread th_8c7ea7bc, 2026-04-19; not copied |
| gemini-2832-prompt | gemini:2832 response → gemini:2832 prompt | Gemini web (lineage), thread th_8c9d9d2b, 2026-03-22; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2833-prompt | gemini:2833 response → gemini:2833 prompt | Gemini web (lineage), thread th_8c9d9d2b, 2026-03-22; not copied |
| gemini-39-prompt | gemini:39 response → gemini:39 prompt | Gemini web (lineage), thread th_8c9f0b1b, 2025-11-25; not copied |
| gemini-673-prompt | gemini:673 response → gemini:673 prompt | Gemini web (lineage), thread th_8ce6030a, 2026-01-12; not copied |
| gemini-674-prompt | gemini:674 response → gemini:674 prompt | Gemini web (lineage), thread th_8ce6030a, 2026-01-12; not copied |
| gemini-675-prompt | gemini:675 response → gemini:675 prompt | Gemini web (lineage), thread th_8ce6030a, 2026-01-12; not copied |
| gemini-676-prompt | gemini:676 response → gemini:676 prompt | Gemini web (lineage), thread th_8ce6030a, 2026-01-12; copied: 1 pasted, 0 lifted into archive notes |
| gemini-677-prompt | gemini:677 response → gemini:677 prompt | Gemini web (lineage), thread th_8ce6030a, 2026-01-12; not copied |
| gemini-678-prompt | gemini:678 response → gemini:678 prompt | Gemini web (lineage), thread th_8ce6030a, 2026-01-12; not copied |
| gemini-679-prompt | gemini:679 response → gemini:679 prompt | Gemini web (lineage), thread th_8ce6030a, 2026-01-12; not copied |
| gemini-2510-prompt | gemini:2510 response → gemini:2510 prompt | Gemini web (lineage), thread th_8d15977b, 2026-03-06; not copied |
| gemini-3103-prompt | gemini:3103 response → gemini:3103 prompt | Gemini web (lineage), thread th_8d1e5fab, 2026-04-08; not copied |
| gemini-2553-prompt | gemini:2553 response → gemini:2553 prompt | Gemini web (lineage), thread th_8e091395, 2026-03-08; not copied |
| gemini-1354-prompt | gemini:1354 response → gemini:1354 prompt | Gemini web (lineage), thread th_8e21eb61, 2026-02-04; not copied |
| gemini-1355-prompt | gemini:1355 response → gemini:1355 prompt | Gemini web (lineage), thread th_8e21eb61, 2026-02-04; not copied |
| gemini-1356-prompt | gemini:1356 response → gemini:1356 prompt | Gemini web (lineage), thread th_8e21eb61, 2026-02-04; not copied |
| gemini-1357-prompt | gemini:1357 response → gemini:1357 prompt | Gemini web (lineage), thread th_8e21eb61, 2026-02-04; copied: 1 pasted, 0 lifted into archive notes |
| gemini-80-prompt | gemini:80 response → gemini:80 prompt | Gemini web (lineage), thread th_8e280791, 2025-12-02; copied: 5 pasted, 1 lifted into archive notes |
| gemini-3026-prompt | gemini:3026 response → gemini:3026 prompt | Gemini web (lineage), thread th_8eb06bb8, 2026-03-31; not copied |
| gemini-3027-prompt | gemini:3027 response → gemini:3027 prompt | Gemini web (lineage), thread th_8eb06bb8, 2026-03-31; not copied |
| gemini-3028-prompt | gemini:3028 response → gemini:3028 prompt | Gemini web (lineage), thread th_8eb06bb8, 2026-03-31; not copied |
| gemini-3029-prompt | gemini:3029 response → gemini:3029 prompt | Gemini web (lineage), thread th_8eb06bb8, 2026-03-31; not copied |
| gemini-3030-prompt | gemini:3030 response → gemini:3030 prompt | Gemini web (lineage), thread th_8eb06bb8, 2026-03-31; not copied |
| gemini-3031-prompt | gemini:3031 response → gemini:3031 prompt | Gemini web (lineage), thread th_8eb06bb8, 2026-03-31; not copied |
| gemini-3032-prompt | gemini:3032 response → gemini:3032 prompt | Gemini web (lineage), thread th_8eb06bb8, 2026-03-31; not copied |
| gemini-3033-prompt | gemini:3033 response → gemini:3033 prompt | Gemini web (lineage), thread th_8eb06bb8, 2026-03-31; not copied |
| gemini-3034-prompt | gemini:3034 response → gemini:3034 prompt | Gemini web (lineage), thread th_8eb06bb8, 2026-03-31; not copied |
| gemini-3035-prompt | gemini:3035 response → gemini:3035 prompt | Gemini web (lineage), thread th_8eb06bb8, 2026-03-31; not copied |
| gemini-3036-prompt | gemini:3036 response → gemini:3036 prompt | Gemini web (lineage), thread th_8eb06bb8, 2026-03-31; not copied |
| gemini-2576-prompt | gemini:2576 response → gemini:2576 prompt | Gemini web (lineage), thread th_8ec44cbd, 2026-03-09; not copied |
| gemini-2061-prompt | gemini:2061 response → gemini:2061 prompt | Gemini web (lineage), thread th_8ef13902, 2026-02-21; not copied |
| gemini-2062-prompt | gemini:2062 response → gemini:2062 prompt | Gemini web (lineage), thread th_8ef13902, 2026-02-21; not copied |
| gemini-40-prompt | gemini:40 response → gemini:40 prompt | Gemini web (lineage), thread th_8ef38923, 2025-11-25; not copied |
| gemini-2317-prompt | gemini:2317 response → gemini:2317 prompt | Gemini web (lineage), thread th_8f3f2f29, 2026-03-03; not copied |
| gemini-2318-prompt | gemini:2318 response → gemini:2318 prompt | Gemini web (lineage), thread th_8f3f2f29, 2026-03-03; not copied |
| gemini-2319-prompt | gemini:2319 response → gemini:2319 prompt | Gemini web (lineage), thread th_8f3f2f29, 2026-03-03; not copied |
| gemini-2320-prompt | gemini:2320 response → gemini:2320 prompt | Gemini web (lineage), thread th_8f3f2f29, 2026-03-03; not copied |
| gemini-2321-prompt | gemini:2321 response → gemini:2321 prompt | Gemini web (lineage), thread th_8f3f2f29, 2026-03-03; not copied |
| gemini-2322-prompt | gemini:2322 response → gemini:2322 prompt | Gemini web (lineage), thread th_8f3f2f29, 2026-03-03; not copied |
| gemini-2323-prompt | gemini:2323 response → gemini:2323 prompt | Gemini web (lineage), thread th_8f3f2f29, 2026-03-03; not copied |
| gemini-2852-prompt | gemini:2852 response → gemini:2852 prompt | Gemini web (lineage), thread th_8f82c983, 2026-03-22; not copied |
| gemini-2785-prompt | gemini:2785 response → gemini:2785 prompt | Gemini web (lineage), thread th_909b845a, 2026-03-20; copied: 3 pasted, 0 lifted into archive notes |
| gemini-1574-prompt | gemini:1574 response → gemini:1574 prompt | Gemini web (lineage), thread th_90a17ef6, 2026-02-09; copied: 1 pasted, 1 lifted into archive notes |
| gemini-327-prompt | gemini:327 response → gemini:327 prompt | Gemini web (lineage), thread th_90f018ba, 2025-12-08; not copied |
| gemini-328-prompt | gemini:328 response → gemini:328 prompt | Gemini web (lineage), thread th_90f018ba, 2025-12-08; not copied |
| gemini-329-prompt | gemini:329 response → gemini:329 prompt | Gemini web (lineage), thread th_90f018ba, 2025-12-08; not copied |
| gemini-204-prompt | gemini:204 response → gemini:204 prompt | Gemini web (lineage), thread th_914df92a, 2025-12-05; not copied |
| gemini-205-prompt | gemini:205 response → gemini:205 prompt | Gemini web (lineage), thread th_914df92a, 2025-12-05; not copied |
| gemini-42-prompt | gemini:42 response → gemini:42 prompt | Gemini web (lineage), thread th_915eab44, 2025-11-25; not copied |
| gemini-43-prompt | gemini:43 response → gemini:43 prompt | Gemini web (lineage), thread th_915eab44, 2025-11-25; not copied |
| gemini-44-prompt | gemini:44 response → gemini:44 prompt | Gemini web (lineage), thread th_915eab44, 2025-11-25; not copied |
| gemini-45-prompt | gemini:45 response → gemini:45 prompt | Gemini web (lineage), thread th_915eab44, 2025-11-25; not copied |
| gemini-46-prompt | gemini:46 response → gemini:46 prompt | Gemini web (lineage), thread th_915eab44, 2025-11-25; not copied |
| gemini-47-prompt | gemini:47 response → gemini:47 prompt | Gemini web (lineage), thread th_915eab44, 2025-11-25; not copied |
| gemini-48-prompt | gemini:48 response → gemini:48 prompt | Gemini web (lineage), thread th_915eab44, 2025-11-25; not copied |
| gemini-49-prompt | gemini:49 response → gemini:49 prompt | Gemini web (lineage), thread th_915eab44, 2025-11-25; not copied |
| gemini-50-prompt | gemini:50 response → gemini:50 prompt | Gemini web (lineage), thread th_915eab44, 2025-11-25; not copied |
| gemini-51-prompt | gemini:51 response → gemini:51 prompt | Gemini web (lineage), thread th_915eab44, 2025-11-25; not copied |
| gemini-52-prompt | gemini:52 response → gemini:52 prompt | Gemini web (lineage), thread th_915eab44, 2025-11-25; not copied |
| gemini-53-prompt | gemini:53 response → gemini:53 prompt | Gemini web (lineage), thread th_915eab44, 2025-11-25; not copied |
| gemini-54-prompt | gemini:54 response → gemini:54 prompt | Gemini web (lineage), thread th_915eab44, 2025-11-25; not copied |
| gemini-55-prompt | gemini:55 response → gemini:55 prompt | Gemini web (lineage), thread th_915eab44, 2025-11-25; not copied |
| gemini-56-prompt | gemini:56 response → gemini:56 prompt | Gemini web (lineage), thread th_915eab44, 2025-11-25; not copied |
| gemini-57-prompt | gemini:57 response → gemini:57 prompt | Gemini web (lineage), thread th_915eab44, 2025-11-25; not copied |
| gemini-2558-prompt | gemini:2558 response → gemini:2558 prompt | Gemini web (lineage), thread th_91624fe2, 2026-03-08; not copied |
| gemini-2559-prompt | gemini:2559 response → gemini:2559 prompt | Gemini web (lineage), thread th_91624fe2, 2026-03-08; not copied |
| gemini-2560-prompt | gemini:2560 response → gemini:2560 prompt | Gemini web (lineage), thread th_91624fe2, 2026-03-08; not copied |
| gemini-2561-prompt | gemini:2561 response → gemini:2561 prompt | Gemini web (lineage), thread th_91624fe2, 2026-03-08; copied: 1 pasted, 0 lifted into archive notes |
| gemini-1193-prompt | gemini:1193 response → gemini:1193 prompt | Gemini web (lineage), thread th_918927df, 2026-01-27 to 2026-01-28; not copied |
| gemini-1194-prompt | gemini:1194 response → gemini:1194 prompt | Gemini web (lineage), thread th_918927df, 2026-01-27 to 2026-01-28; not copied |
| gemini-1195-prompt | gemini:1195 response → gemini:1195 prompt | Gemini web (lineage), thread th_918927df, 2026-01-27 to 2026-01-28; not copied |
| gemini-1196-prompt | gemini:1196 response → gemini:1196 prompt | Gemini web (lineage), thread th_918927df, 2026-01-27 to 2026-01-28; not copied |
| gemini-1197-prompt | gemini:1197 response → gemini:1197 prompt | Gemini web (lineage), thread th_918927df, 2026-01-27 to 2026-01-28; copied: 7 pasted, 0 lifted into archive notes |
| gemini-1003-prompt | gemini:1003 response → gemini:1003 prompt | Gemini web (lineage), thread th_91a35af3, 2026-01-24; not copied |
| gemini-2011-prompt | gemini:2011 response → gemini:2011 prompt | Gemini web (lineage), thread th_91f8734b, 2026-02-20; not copied |
| gemini-2012-prompt | gemini:2012 response → gemini:2012 prompt | Gemini web (lineage), thread th_91f8734b, 2026-02-20; not copied |
| gemini-2013-prompt | gemini:2013 response → gemini:2013 prompt | Gemini web (lineage), thread th_91f8734b, 2026-02-20; not copied |
| gemini-2014-prompt | gemini:2014 response → gemini:2014 prompt | Gemini web (lineage), thread th_91f8734b, 2026-02-20; not copied |
| gemini-3088-prompt | gemini:3088 response → gemini:3088 prompt | Gemini web (lineage), thread th_928e9c7f, 2026-04-06; not copied |
| gemini-425-prompt | gemini:425 response → gemini:425 prompt | Gemini web (lineage), thread th_92c645b5, 2025-12-28; not copied |
| gemini-1832-prompt | gemini:1832 response → gemini:1832 prompt | Gemini web (lineage), thread th_9304955b, 2026-02-13; not copied |
| gemini-1833-prompt | gemini:1833 response → gemini:1833 prompt | Gemini web (lineage), thread th_9304955b, 2026-02-13; not copied |
| gemini-995-prompt | gemini:995 response → gemini:995 prompt | Gemini web (lineage), thread th_9330c411, 2026-01-23; not copied |
| gemini-996-prompt | gemini:996 response → gemini:996 prompt | Gemini web (lineage), thread th_9330c411, 2026-01-23; not copied |
| gemini-997-prompt | gemini:997 response → gemini:997 prompt | Gemini web (lineage), thread th_9330c411, 2026-01-23; not copied |
| gemini-998-prompt | gemini:998 response → gemini:998 prompt | Gemini web (lineage), thread th_9330c411, 2026-01-23; not copied |
| gemini-144-prompt | gemini:144 response → gemini:144 prompt | Gemini web (lineage), thread th_9339891d, 2025-12-04; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2744-prompt | gemini:2744 response → gemini:2744 prompt | Gemini web (lineage), thread th_934e1975, 2026-03-17; not copied |
| gemini-2745-prompt | gemini:2745 response → gemini:2745 prompt | Gemini web (lineage), thread th_934e1975, 2026-03-17; not copied |
| gemini-2746-prompt | gemini:2746 response → gemini:2746 prompt | Gemini web (lineage), thread th_934e1975, 2026-03-17; not copied |
| gemini-584-prompt | gemini:584 response → gemini:584 prompt | Gemini web (lineage), thread th_9352281a, 2026-01-10; not copied |
| gemini-585-prompt | gemini:585 response → gemini:585 prompt | Gemini web (lineage), thread th_9352281a, 2026-01-10; not copied |
| gemini-586-prompt | gemini:586 response → gemini:586 prompt | Gemini web (lineage), thread th_9352281a, 2026-01-10; not copied |
| gemini-587-prompt | gemini:587 response → gemini:587 prompt | Gemini web (lineage), thread th_9352281a, 2026-01-10; not copied |
| gemini-588-prompt | gemini:588 response → gemini:588 prompt | Gemini web (lineage), thread th_9352281a, 2026-01-10; not copied |
| gemini-589-prompt | gemini:589 response → gemini:589 prompt | Gemini web (lineage), thread th_9352281a, 2026-01-10; not copied |
| gemini-590-prompt | gemini:590 response → gemini:590 prompt | Gemini web (lineage), thread th_9352281a, 2026-01-10; not copied |
| gemini-591-prompt | gemini:591 response → gemini:591 prompt | Gemini web (lineage), thread th_9352281a, 2026-01-10; not copied |
| gemini-592-prompt | gemini:592 response → gemini:592 prompt | Gemini web (lineage), thread th_9352281a, 2026-01-10; not copied |
| gemini-1952-prompt | gemini:1952 response → gemini:1952 prompt | Gemini web (lineage), thread th_9369f73f, 2026-02-20; not copied |
| gemini-1953-prompt | gemini:1953 response → gemini:1953 prompt | Gemini web (lineage), thread th_9369f73f, 2026-02-20; copied: 1 pasted, 0 lifted into archive notes |
| gemini-1954-prompt | gemini:1954 response → gemini:1954 prompt | Gemini web (lineage), thread th_9369f73f, 2026-02-20; copied: 3 pasted, 0 lifted into archive notes |
| gemini-1955-prompt | gemini:1955 response → gemini:1955 prompt | Gemini web (lineage), thread th_9369f73f, 2026-02-20; copied: 1 pasted, 0 lifted into archive notes |
| gemini-1956-prompt | gemini:1956 response → gemini:1956 prompt | Gemini web (lineage), thread th_9369f73f, 2026-02-20; not copied |
| gemini-1957-prompt | gemini:1957 response → gemini:1957 prompt | Gemini web (lineage), thread th_9369f73f, 2026-02-20; not copied |
| gemini-1958-prompt | gemini:1958 response → gemini:1958 prompt | Gemini web (lineage), thread th_9369f73f, 2026-02-20; not copied |
| gemini-1959-prompt | gemini:1959 response → gemini:1959 prompt | Gemini web (lineage), thread th_9369f73f, 2026-02-20; not copied |
| gemini-1960-prompt | gemini:1960 response → gemini:1960 prompt | Gemini web (lineage), thread th_9369f73f, 2026-02-20; not copied |
| gemini-1961-prompt | gemini:1961 response → gemini:1961 prompt | Gemini web (lineage), thread th_9369f73f, 2026-02-20; not copied |
| gemini-423-prompt | gemini:423 response → gemini:423 prompt | Gemini web (lineage), thread th_942c376e, 2025-12-28; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2309-prompt | gemini:2309 response → gemini:2309 prompt | Gemini web (lineage), thread th_94350208, 2026-03-01; not copied |
| gemini-2908-prompt | gemini:2908 response → gemini:2908 prompt | Gemini web (lineage), thread th_943b29bc, 2026-03-24; not copied |
| gemini-2909-prompt | gemini:2909 response → gemini:2909 prompt | Gemini web (lineage), thread th_943b29bc, 2026-03-24; not copied |
| gemini-2910-prompt | gemini:2910 response → gemini:2910 prompt | Gemini web (lineage), thread th_943b29bc, 2026-03-24; not copied |
| gemini-2911-prompt | gemini:2911 response → gemini:2911 prompt | Gemini web (lineage), thread th_943b29bc, 2026-03-24; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2912-prompt | gemini:2912 response → gemini:2912 prompt | Gemini web (lineage), thread th_943b29bc, 2026-03-24; not copied |
| gemini-2913-prompt | gemini:2913 response → gemini:2913 prompt | Gemini web (lineage), thread th_943b29bc, 2026-03-24; copied: 0 pasted, 1 lifted into archive notes |
| gemini-2914-prompt | gemini:2914 response → gemini:2914 prompt | Gemini web (lineage), thread th_943b29bc, 2026-03-24; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2915-prompt | gemini:2915 response → gemini:2915 prompt | Gemini web (lineage), thread th_943b29bc, 2026-03-24; not copied |
| gemini-1737-prompt | gemini:1737 response → gemini:1737 prompt | Gemini web (lineage), thread th_948029f5, 2026-02-11; not copied |
| gemini-1738-prompt | gemini:1738 response → gemini:1738 prompt | Gemini web (lineage), thread th_948029f5, 2026-02-11; not copied |
| gemini-3210-prompt | gemini:3210 response → gemini:3210 prompt | Gemini web (lineage), thread th_9490d25e, 2026-04-19; not copied |
| gemini-3211-prompt | gemini:3211 response → gemini:3211 prompt | Gemini web (lineage), thread th_9490d25e, 2026-04-19; not copied |
| gemini-3212-prompt | gemini:3212 response → gemini:3212 prompt | Gemini web (lineage), thread th_9490d25e, 2026-04-19; not copied |
| gemini-3213-prompt | gemini:3213 response → gemini:3213 prompt | Gemini web (lineage), thread th_9490d25e, 2026-04-19; not copied |
| gemini-3214-prompt | gemini:3214 response → gemini:3214 prompt | Gemini web (lineage), thread th_9490d25e, 2026-04-19; not copied |
| gemini-3215-prompt | gemini:3215 response → gemini:3215 prompt | Gemini web (lineage), thread th_9490d25e, 2026-04-19; not copied |
| gemini-3216-prompt | gemini:3216 response → gemini:3216 prompt | Gemini web (lineage), thread th_9490d25e, 2026-04-19; not copied |
| gemini-3217-prompt | gemini:3217 response → gemini:3217 prompt | Gemini web (lineage), thread th_9490d25e, 2026-04-19; not copied |
| gemini-3218-prompt | gemini:3218 response → gemini:3218 prompt | Gemini web (lineage), thread th_9490d25e, 2026-04-19; not copied |
| gemini-3219-prompt | gemini:3219 response → gemini:3219 prompt | Gemini web (lineage), thread th_9490d25e, 2026-04-19; not copied |
| gemini-3220-prompt | gemini:3220 response → gemini:3220 prompt | Gemini web (lineage), thread th_9490d25e, 2026-04-19; not copied |
| gemini-3221-prompt | gemini:3221 response → gemini:3221 prompt | Gemini web (lineage), thread th_9490d25e, 2026-04-19; not copied |
| gemini-3222-prompt | gemini:3222 response → gemini:3222 prompt | Gemini web (lineage), thread th_9490d25e, 2026-04-19; not copied |
| gemini-3223-prompt | gemini:3223 response → gemini:3223 prompt | Gemini web (lineage), thread th_9490d25e, 2026-04-19; not copied |
| gemini-3224-prompt | gemini:3224 response → gemini:3224 prompt | Gemini web (lineage), thread th_9490d25e, 2026-04-19; not copied |
| gemini-3225-prompt | gemini:3225 response → gemini:3225 prompt | Gemini web (lineage), thread th_9490d25e, 2026-04-19; not copied |
| gemini-3226-prompt | gemini:3226 response → gemini:3226 prompt | Gemini web (lineage), thread th_9490d25e, 2026-04-19; not copied |
| gemini-3227-prompt | gemini:3227 response → gemini:3227 prompt | Gemini web (lineage), thread th_9490d25e, 2026-04-19; not copied |
| gemini-3228-prompt | gemini:3228 response → gemini:3228 prompt | Gemini web (lineage), thread th_9490d25e, 2026-04-19; not copied |
| gemini-3229-prompt | gemini:3229 response → gemini:3229 prompt | Gemini web (lineage), thread th_9490d25e, 2026-04-19; not copied |
| gemini-3230-prompt | gemini:3230 response → gemini:3230 prompt | Gemini web (lineage), thread th_9490d25e, 2026-04-19; not copied |
| gemini-1345-prompt | gemini:1345 response → gemini:1345 prompt | Gemini web (lineage), thread th_94e61e7d, 2026-02-04; copied: 2 pasted, 0 lifted into archive notes |
| gemini-1346-prompt | gemini:1346 response → gemini:1346 prompt | Gemini web (lineage), thread th_94e61e7d, 2026-02-04; copied: 2 pasted, 1 lifted into archive notes |
| gemini-1712-prompt | gemini:1712 response → gemini:1712 prompt | Gemini web (lineage), thread th_94ed3093, 2026-02-11; not copied |
| gemini-1713-prompt | gemini:1713 response → gemini:1713 prompt | Gemini web (lineage), thread th_94ed3093, 2026-02-11; not copied |
| gemini-1714-prompt | gemini:1714 response → gemini:1714 prompt | Gemini web (lineage), thread th_94ed3093, 2026-02-11; not copied |
| gemini-1715-prompt | gemini:1715 response → gemini:1715 prompt | Gemini web (lineage), thread th_94ed3093, 2026-02-11; not copied |
| gemini-1716-prompt | gemini:1716 response → gemini:1716 prompt | Gemini web (lineage), thread th_94ed3093, 2026-02-11; not copied |
| gemini-1717-prompt | gemini:1717 response → gemini:1717 prompt | Gemini web (lineage), thread th_94ed3093, 2026-02-11; not copied |
| gemini-1718-prompt | gemini:1718 response → gemini:1718 prompt | Gemini web (lineage), thread th_94ed3093, 2026-02-11; not copied |
| gemini-1719-prompt | gemini:1719 response → gemini:1719 prompt | Gemini web (lineage), thread th_94ed3093, 2026-02-11; not copied |
| gemini-1720-prompt | gemini:1720 response → gemini:1720 prompt | Gemini web (lineage), thread th_94ed3093, 2026-02-11; not copied |
| gemini-1721-prompt | gemini:1721 response → gemini:1721 prompt | Gemini web (lineage), thread th_94ed3093, 2026-02-11; not copied |
| gemini-2562-prompt | gemini:2562 response → gemini:2562 prompt | Gemini web (lineage), thread th_968c36f0, 2026-03-09; copied: 4 pasted, 0 lifted into archive notes |
| gemini-2563-prompt | gemini:2563 response → gemini:2563 prompt | Gemini web (lineage), thread th_968c36f0, 2026-03-09; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2564-prompt | gemini:2564 response → gemini:2564 prompt | Gemini web (lineage), thread th_968c36f0, 2026-03-09; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2565-prompt | gemini:2565 response → gemini:2565 prompt | Gemini web (lineage), thread th_968c36f0, 2026-03-09; copied: 0 pasted, 1 lifted into archive notes |
| gemini-2566-prompt | gemini:2566 response → gemini:2566 prompt | Gemini web (lineage), thread th_968c36f0, 2026-03-09; not copied |
| gemini-1165-prompt | gemini:1165 response → gemini:1165 prompt | Gemini web (lineage), thread th_96992014, 2026-01-27; not copied |
| gemini-1166-prompt | gemini:1166 response → gemini:1166 prompt | Gemini web (lineage), thread th_96992014, 2026-01-27; copied: 1 pasted, 0 lifted into archive notes |
| gemini-1167-prompt | gemini:1167 response → gemini:1167 prompt | Gemini web (lineage), thread th_96992014, 2026-01-27; copied: 1 pasted, 0 lifted into archive notes |
| gemini-1168-prompt | gemini:1168 response → gemini:1168 prompt | Gemini web (lineage), thread th_96992014, 2026-01-27; not copied |
| gemini-1169-prompt | gemini:1169 response → gemini:1169 prompt | Gemini web (lineage), thread th_96992014, 2026-01-27; not copied |
| gemini-1170-prompt | gemini:1170 response → gemini:1170 prompt | Gemini web (lineage), thread th_96992014, 2026-01-27; not copied |
| gemini-1171-prompt | gemini:1171 response → gemini:1171 prompt | Gemini web (lineage), thread th_96992014, 2026-01-27; not copied |
| gemini-1172-prompt | gemini:1172 response → gemini:1172 prompt | Gemini web (lineage), thread th_96992014, 2026-01-27; copied: 2 pasted, 0 lifted into archive notes |
| gemini-1173-prompt | gemini:1173 response → gemini:1173 prompt | Gemini web (lineage), thread th_96992014, 2026-01-27; copied: 2 pasted, 0 lifted into archive notes |
| gemini-1174-prompt | gemini:1174 response → gemini:1174 prompt | Gemini web (lineage), thread th_96992014, 2026-01-27; copied: 11 pasted, 1 lifted into archive notes |
| gemini-1175-prompt | gemini:1175 response → gemini:1175 prompt | Gemini web (lineage), thread th_96992014, 2026-01-27; not copied |
| gemini-1176-prompt | gemini:1176 response → gemini:1176 prompt | Gemini web (lineage), thread th_96992014, 2026-01-27; not copied |
| gemini-1177-prompt | gemini:1177 response → gemini:1177 prompt | Gemini web (lineage), thread th_96992014, 2026-01-27; copied: 2 pasted, 0 lifted into archive notes |
| gemini-1178-prompt | gemini:1178 response → gemini:1178 prompt | Gemini web (lineage), thread th_96992014, 2026-01-27; copied: 1 pasted, 0 lifted into archive notes |
| gemini-1179-prompt | gemini:1179 response → gemini:1179 prompt | Gemini web (lineage), thread th_96992014, 2026-01-27; copied: 1 pasted, 0 lifted into archive notes |
| gemini-428-prompt | gemini:428 response → gemini:428 prompt | Gemini web (lineage), thread th_97458911, 2025-12-29; not copied |
| gemini-2432-prompt | gemini:2432 response → gemini:2432 prompt | Gemini web (lineage), thread th_97948aa6, 2026-03-05; not copied |
| gemini-2433-prompt | gemini:2433 response → gemini:2433 prompt | Gemini web (lineage), thread th_97948aa6, 2026-03-05; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2434-prompt | gemini:2434 response → gemini:2434 prompt | Gemini web (lineage), thread th_97948aa6, 2026-03-05; not copied |
| gemini-2435-prompt | gemini:2435 response → gemini:2435 prompt | Gemini web (lineage), thread th_97948aa6, 2026-03-05; not copied |
| gemini-621-prompt | gemini:621 response → gemini:621 prompt | Gemini web (lineage), thread th_97d2f0c0, 2026-01-10; not copied |
| gemini-647-prompt | gemini:647 response → gemini:647 prompt | Gemini web (lineage), thread th_97d6bf7c, 2026-01-11; not copied |
| gemini-648-prompt | gemini:648 response → gemini:648 prompt | Gemini web (lineage), thread th_97d6bf7c, 2026-01-11; not copied |
| gemini-2645-prompt | gemini:2645 response → gemini:2645 prompt | Gemini web (lineage), thread th_98769cd5, 2026-03-10; copied: 2 pasted, 0 lifted into archive notes |
| gemini-2646-prompt | gemini:2646 response → gemini:2646 prompt | Gemini web (lineage), thread th_98769cd5, 2026-03-10; not copied |
| gemini-2647-prompt | gemini:2647 response → gemini:2647 prompt | Gemini web (lineage), thread th_98769cd5, 2026-03-10; copied: 0 pasted, 1 lifted into archive notes |
| gemini-2648-prompt | gemini:2648 response → gemini:2648 prompt | Gemini web (lineage), thread th_98769cd5, 2026-03-10; not copied |
| gemini-1392-prompt | gemini:1392 response → gemini:1392 prompt | Gemini web (lineage), thread th_98a41a36, 2026-02-06; copied: 2 pasted, 0 lifted into archive notes |
| gemini-735-prompt | gemini:735 response → gemini:735 prompt | Gemini web (lineage), thread th_98a5b78b, 2026-01-14; not copied |
| gemini-736-prompt | gemini:736 response → gemini:736 prompt | Gemini web (lineage), thread th_98a5b78b, 2026-01-14; not copied |
| gemini-737-prompt | gemini:737 response → gemini:737 prompt | Gemini web (lineage), thread th_98a5b78b, 2026-01-14; not copied |
| gemini-738-prompt | gemini:738 response → gemini:738 prompt | Gemini web (lineage), thread th_98a5b78b, 2026-01-14; not copied |
| gemini-739-prompt | gemini:739 response → gemini:739 prompt | Gemini web (lineage), thread th_98a5b78b, 2026-01-14; not copied |
| gemini-740-prompt | gemini:740 response → gemini:740 prompt | Gemini web (lineage), thread th_98a5b78b, 2026-01-14; not copied |
| gemini-741-prompt | gemini:741 response → gemini:741 prompt | Gemini web (lineage), thread th_98a5b78b, 2026-01-14; not copied |
| gemini-742-prompt | gemini:742 response → gemini:742 prompt | Gemini web (lineage), thread th_98a5b78b, 2026-01-14; not copied |
| gemini-2080-prompt | gemini:2080 response → gemini:2080 prompt | Gemini web (lineage), thread th_98edcfa1, 2026-02-24; not copied |
| gemini-2081-prompt | gemini:2081 response → gemini:2081 prompt | Gemini web (lineage), thread th_98edcfa1, 2026-02-24; not copied |
| gemini-2082-prompt | gemini:2082 response → gemini:2082 prompt | Gemini web (lineage), thread th_98edcfa1, 2026-02-24; not copied |
| gemini-2083-prompt | gemini:2083 response → gemini:2083 prompt | Gemini web (lineage), thread th_98edcfa1, 2026-02-24; not copied |
| gemini-2084-prompt | gemini:2084 response → gemini:2084 prompt | Gemini web (lineage), thread th_98edcfa1, 2026-02-24; not copied |
| gemini-2085-prompt | gemini:2085 response → gemini:2085 prompt | Gemini web (lineage), thread th_98edcfa1, 2026-02-24; not copied |
| gemini-2086-prompt | gemini:2086 response → gemini:2086 prompt | Gemini web (lineage), thread th_98edcfa1, 2026-02-24; not copied |
| gemini-2087-prompt | gemini:2087 response → gemini:2087 prompt | Gemini web (lineage), thread th_98edcfa1, 2026-02-24; not copied |
| gemini-830-prompt | gemini:830 response → gemini:830 prompt | Gemini web (lineage), thread th_98fa1b1b, 2026-01-19; not copied |
| gemini-831-prompt | gemini:831 response → gemini:831 prompt | Gemini web (lineage), thread th_98fa1b1b, 2026-01-19; not copied |
| gemini-832-prompt | gemini:832 response → gemini:832 prompt | Gemini web (lineage), thread th_98fa1b1b, 2026-01-19; not copied |
| gemini-833-prompt | gemini:833 response → gemini:833 prompt | Gemini web (lineage), thread th_98fa1b1b, 2026-01-19; not copied |
| gemini-834-prompt | gemini:834 response → gemini:834 prompt | Gemini web (lineage), thread th_98fa1b1b, 2026-01-19; not copied |
| gemini-835-prompt | gemini:835 response → gemini:835 prompt | Gemini web (lineage), thread th_98fa1b1b, 2026-01-19; not copied |
| gemini-836-prompt | gemini:836 response → gemini:836 prompt | Gemini web (lineage), thread th_98fa1b1b, 2026-01-19; not copied |
| gemini-837-prompt | gemini:837 response → gemini:837 prompt | Gemini web (lineage), thread th_98fa1b1b, 2026-01-19; not copied |
| gemini-838-prompt | gemini:838 response → gemini:838 prompt | Gemini web (lineage), thread th_98fa1b1b, 2026-01-19; not copied |
| gemini-839-prompt | gemini:839 response → gemini:839 prompt | Gemini web (lineage), thread th_98fa1b1b, 2026-01-19; not copied |
| gemini-840-prompt | gemini:840 response → gemini:840 prompt | Gemini web (lineage), thread th_98fa1b1b, 2026-01-19; not copied |
| gemini-841-prompt | gemini:841 response → gemini:841 prompt | Gemini web (lineage), thread th_98fa1b1b, 2026-01-19; not copied |
| gemini-842-prompt | gemini:842 response → gemini:842 prompt | Gemini web (lineage), thread th_98fa1b1b, 2026-01-19; not copied |
| gemini-843-prompt | gemini:843 response → gemini:843 prompt | Gemini web (lineage), thread th_98fa1b1b, 2026-01-19; not copied |
| gemini-844-prompt | gemini:844 response → gemini:844 prompt | Gemini web (lineage), thread th_98fa1b1b, 2026-01-19; not copied |
| gemini-845-prompt | gemini:845 response → gemini:845 prompt | Gemini web (lineage), thread th_98fa1b1b, 2026-01-19; not copied |
| gemini-846-prompt | gemini:846 response → gemini:846 prompt | Gemini web (lineage), thread th_98fa1b1b, 2026-01-19; not copied |
| gemini-650-prompt | gemini:650 response → gemini:650 prompt | Gemini web (lineage), thread th_9916e01a, 2026-01-11 to 2026-01-12; not copied |
| gemini-651-prompt | gemini:651 response → gemini:651 prompt | Gemini web (lineage), thread th_9916e01a, 2026-01-11 to 2026-01-12; not copied |
| gemini-652-prompt | gemini:652 response → gemini:652 prompt | Gemini web (lineage), thread th_9916e01a, 2026-01-11 to 2026-01-12; not copied |
| gemini-653-prompt | gemini:653 response → gemini:653 prompt | Gemini web (lineage), thread th_9916e01a, 2026-01-11 to 2026-01-12; copied: 0 pasted, 1 lifted into archive notes |
| gemini-654-prompt | gemini:654 response → gemini:654 prompt | Gemini web (lineage), thread th_9916e01a, 2026-01-11 to 2026-01-12; not copied |
| gemini-655-prompt | gemini:655 response → gemini:655 prompt | Gemini web (lineage), thread th_9916e01a, 2026-01-11 to 2026-01-12; not copied |
| gemini-192-prompt | gemini:192 response → gemini:192 prompt | Gemini web (lineage), thread th_992141dd, 2025-12-04; not copied |
| gemini-3007-prompt | gemini:3007 response → gemini:3007 prompt | Gemini web (lineage), thread th_9950936e, 2026-03-26; not copied |
| gemini-2116-prompt | gemini:2116 response → gemini:2116 prompt | Gemini web (lineage), thread th_995a771a, 2026-02-25; not copied |
| gemini-413-prompt | gemini:413 response → gemini:413 prompt | Gemini web (lineage), thread th_99758cf7, 2025-12-28; not copied |
| gemini-504-prompt | gemini:504 response → gemini:504 prompt | Gemini web (lineage), thread th_9a1a010b, 2026-01-09; not copied |
| gemini-505-prompt | gemini:505 response → gemini:505 prompt | Gemini web (lineage), thread th_9a1a010b, 2026-01-09; copied: 0 pasted, 1 lifted into archive notes |
| gemini-506-prompt | gemini:506 response → gemini:506 prompt | Gemini web (lineage), thread th_9a1a010b, 2026-01-09; not copied |
| gemini-1260-prompt | gemini:1260 response → gemini:1260 prompt | Gemini web (lineage), thread th_9a288fa5, 2026-01-31; not copied |
| gemini-1261-prompt | gemini:1261 response → gemini:1261 prompt | Gemini web (lineage), thread th_9a288fa5, 2026-01-31; not copied |
| gemini-1262-prompt | gemini:1262 response → gemini:1262 prompt | Gemini web (lineage), thread th_9a288fa5, 2026-01-31; not copied |
| gemini-1263-prompt | gemini:1263 response → gemini:1263 prompt | Gemini web (lineage), thread th_9a288fa5, 2026-01-31; not copied |
| gemini-1264-prompt | gemini:1264 response → gemini:1264 prompt | Gemini web (lineage), thread th_9a288fa5, 2026-01-31; not copied |
| gemini-1265-prompt | gemini:1265 response → gemini:1265 prompt | Gemini web (lineage), thread th_9a288fa5, 2026-01-31; not copied |
| gemini-1266-prompt | gemini:1266 response → gemini:1266 prompt | Gemini web (lineage), thread th_9a288fa5, 2026-01-31; not copied |
| gemini-1267-prompt | gemini:1267 response → gemini:1267 prompt | Gemini web (lineage), thread th_9a288fa5, 2026-01-31; not copied |
| gemini-2835-prompt | gemini:2835 response → gemini:2835 prompt | Gemini web (lineage), thread th_9a8a1151, 2026-03-22; not copied |
| gemini-2836-prompt | gemini:2836 response → gemini:2836 prompt | Gemini web (lineage), thread th_9a8a1151, 2026-03-22; not copied |
| gemini-2837-prompt | gemini:2837 response → gemini:2837 prompt | Gemini web (lineage), thread th_9a8a1151, 2026-03-22; not copied |
| gemini-2838-prompt | gemini:2838 response → gemini:2838 prompt | Gemini web (lineage), thread th_9a8a1151, 2026-03-22; not copied |
| gemini-368-prompt | gemini:368 response → gemini:368 prompt | Gemini web (lineage), thread th_9b5ac24c, 2025-12-28; not copied |
| gemini-369-prompt | gemini:369 response → gemini:369 prompt | Gemini web (lineage), thread th_9b5ac24c, 2025-12-28; not copied |
| gemini-899-prompt | gemini:899 response → gemini:899 prompt | Gemini web (lineage), thread th_9c595cc1, 2026-01-22; not copied |
| gemini-900-prompt | gemini:900 response → gemini:900 prompt | Gemini web (lineage), thread th_9c595cc1, 2026-01-22; not copied |
| gemini-901-prompt | gemini:901 response → gemini:901 prompt | Gemini web (lineage), thread th_9c595cc1, 2026-01-22; not copied |
| gemini-902-prompt | gemini:902 response → gemini:902 prompt | Gemini web (lineage), thread th_9c595cc1, 2026-01-22; not copied |
| gemini-903-prompt | gemini:903 response → gemini:903 prompt | Gemini web (lineage), thread th_9c595cc1, 2026-01-22; not copied |
| gemini-3057-prompt | gemini:3057 response → gemini:3057 prompt | Gemini web (lineage), thread th_9c833f1e, 2026-04-01; not copied |
| gemini-3058-prompt | gemini:3058 response → gemini:3058 prompt | Gemini web (lineage), thread th_9c833f1e, 2026-04-01; not copied |
| gemini-3059-prompt | gemini:3059 response → gemini:3059 prompt | Gemini web (lineage), thread th_9c833f1e, 2026-04-01; not copied |
| gemini-3060-prompt | gemini:3060 response → gemini:3060 prompt | Gemini web (lineage), thread th_9c833f1e, 2026-04-01; not copied |
| gemini-3061-prompt | gemini:3061 response → gemini:3061 prompt | Gemini web (lineage), thread th_9c833f1e, 2026-04-01; not copied |
| gemini-3062-prompt | gemini:3062 response → gemini:3062 prompt | Gemini web (lineage), thread th_9c833f1e, 2026-04-01; not copied |
| gemini-3063-prompt | gemini:3063 response → gemini:3063 prompt | Gemini web (lineage), thread th_9c833f1e, 2026-04-01; not copied |
| gemini-3064-prompt | gemini:3064 response → gemini:3064 prompt | Gemini web (lineage), thread th_9c833f1e, 2026-04-01; not copied |
| gemini-3065-prompt | gemini:3065 response → gemini:3065 prompt | Gemini web (lineage), thread th_9c833f1e, 2026-04-01; not copied |
| gemini-2554-prompt | gemini:2554 response → gemini:2554 prompt | Gemini web (lineage), thread th_9caac3b9, 2026-03-08; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2555-prompt | gemini:2555 response → gemini:2555 prompt | Gemini web (lineage), thread th_9caac3b9, 2026-03-08; copied: 1 pasted, 0 lifted into archive notes |
| gemini-1787-prompt | gemini:1787 response → gemini:1787 prompt | Gemini web (lineage), thread th_9cdc9874, 2026-02-13; not copied |
| gemini-2665-prompt | gemini:2665 response → gemini:2665 prompt | Gemini web (lineage), thread th_9cf7923c, 2026-03-12; not copied |
| gemini-2666-prompt | gemini:2666 response → gemini:2666 prompt | Gemini web (lineage), thread th_9cf7923c, 2026-03-12; not copied |
| gemini-2667-prompt | gemini:2667 response → gemini:2667 prompt | Gemini web (lineage), thread th_9cf7923c, 2026-03-12; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2668-prompt | gemini:2668 response → gemini:2668 prompt | Gemini web (lineage), thread th_9cf7923c, 2026-03-12; not copied |
| gemini-2669-prompt | gemini:2669 response → gemini:2669 prompt | Gemini web (lineage), thread th_9cf7923c, 2026-03-12; copied: 2 pasted, 0 lifted into archive notes |
| gemini-2670-prompt | gemini:2670 response → gemini:2670 prompt | Gemini web (lineage), thread th_9cf7923c, 2026-03-12; not copied |
| gemini-2671-prompt | gemini:2671 response → gemini:2671 prompt | Gemini web (lineage), thread th_9cf7923c, 2026-03-12; not copied |
| gemini-2672-prompt | gemini:2672 response → gemini:2672 prompt | Gemini web (lineage), thread th_9cf7923c, 2026-03-12; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2673-prompt | gemini:2673 response → gemini:2673 prompt | Gemini web (lineage), thread th_9cf7923c, 2026-03-12; not copied |
| gemini-2674-prompt | gemini:2674 response → gemini:2674 prompt | Gemini web (lineage), thread th_9cf7923c, 2026-03-12; not copied |
| gemini-2675-prompt | gemini:2675 response → gemini:2675 prompt | Gemini web (lineage), thread th_9cf7923c, 2026-03-12; copied: 1 pasted, 0 lifted into archive notes |
| gemini-1008-prompt | gemini:1008 response → gemini:1008 prompt | Gemini web (lineage), thread th_9dca7ca5, 2026-01-24; not copied |
| gemini-2953-prompt | gemini:2953 response → gemini:2953 prompt | Gemini web (lineage), thread th_9ed93a5e, 2026-03-25; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2954-prompt | gemini:2954 response → gemini:2954 prompt | Gemini web (lineage), thread th_9ed93a5e, 2026-03-25; not copied |
| gemini-2955-prompt | gemini:2955 response → gemini:2955 prompt | Gemini web (lineage), thread th_9ed93a5e, 2026-03-25; not copied |
| gemini-2956-prompt | gemini:2956 response → gemini:2956 prompt | Gemini web (lineage), thread th_9ed93a5e, 2026-03-25; not copied |
| gemini-2957-prompt | gemini:2957 response → gemini:2957 prompt | Gemini web (lineage), thread th_9ed93a5e, 2026-03-25; not copied |
| gemini-2958-prompt | gemini:2958 response → gemini:2958 prompt | Gemini web (lineage), thread th_9ed93a5e, 2026-03-25; not copied |
| gemini-3084-prompt | gemini:3084 response → gemini:3084 prompt | Gemini web (lineage), thread th_9eeaeb07, 2026-04-01; not copied |
| gemini-61-prompt | gemini:61 response → gemini:61 prompt | Gemini web (lineage), thread th_9f515a3a, 2025-11-26; not copied |
| gemini-62-prompt | gemini:62 response → gemini:62 prompt | Gemini web (lineage), thread th_9f515a3a, 2025-11-26; copied: 0 pasted, 1 lifted into archive notes |
| gemini-2864-prompt | gemini:2864 response → gemini:2864 prompt | Gemini web (lineage), thread th_9f979e9d, 2026-03-23; not copied |
| gemini-2865-prompt | gemini:2865 response → gemini:2865 prompt | Gemini web (lineage), thread th_9f979e9d, 2026-03-23; copied: 1 pasted, 0 lifted into archive notes |
| gemini-3134-prompt | gemini:3134 response → gemini:3134 prompt | Gemini web (lineage), thread th_a0378aed, 2026-04-09; not copied |
| gemini-1827-prompt | gemini:1827 response → gemini:1827 prompt | Gemini web (lineage), thread th_a05b6c0e, 2026-02-13; not copied |
| gemini-1828-prompt | gemini:1828 response → gemini:1828 prompt | Gemini web (lineage), thread th_a05b6c0e, 2026-02-13; not copied |
| gemini-1829-prompt | gemini:1829 response → gemini:1829 prompt | Gemini web (lineage), thread th_a05b6c0e, 2026-02-13; not copied |
| gemini-1830-prompt | gemini:1830 response → gemini:1830 prompt | Gemini web (lineage), thread th_a05b6c0e, 2026-02-13; not copied |
| gemini-1831-prompt | gemini:1831 response → gemini:1831 prompt | Gemini web (lineage), thread th_a05b6c0e, 2026-02-13; not copied |
| gemini-1949-prompt | gemini:1949 response → gemini:1949 prompt | Gemini web (lineage), thread th_a05ed566, 2026-02-20; copied: 1 pasted, 0 lifted into archive notes |
| gemini-1950-prompt | gemini:1950 response → gemini:1950 prompt | Gemini web (lineage), thread th_a05ed566, 2026-02-20; not copied |
| gemini-1951-prompt | gemini:1951 response → gemini:1951 prompt | Gemini web (lineage), thread th_a05ed566, 2026-02-20; not copied |
| gemini-1363-prompt | gemini:1363 response → gemini:1363 prompt | Gemini web (lineage), thread th_a0725604, 2026-02-05; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2557-prompt | gemini:2557 response → gemini:2557 prompt | Gemini web (lineage), thread th_a10b134e, 2026-03-08; not copied |
| gemini-3163-prompt | gemini:3163 response → gemini:3163 prompt | Gemini web (lineage), thread th_a1c4dbc7, 2026-04-15; not copied |
| gemini-3164-prompt | gemini:3164 response → gemini:3164 prompt | Gemini web (lineage), thread th_a1c4dbc7, 2026-04-15; not copied |
| gemini-3165-prompt | gemini:3165 response → gemini:3165 prompt | Gemini web (lineage), thread th_a1c4dbc7, 2026-04-15; not copied |
| gemini-3166-prompt | gemini:3166 response → gemini:3166 prompt | Gemini web (lineage), thread th_a1c4dbc7, 2026-04-15; not copied |
| gemini-1070-prompt | gemini:1070 response → gemini:1070 prompt | Gemini web (lineage), thread th_a234a9df, 2026-01-25; copied: 1 pasted, 0 lifted into archive notes |
| gemini-1071-prompt | gemini:1071 response → gemini:1071 prompt | Gemini web (lineage), thread th_a234a9df, 2026-01-25; not copied |
| gemini-1072-prompt | gemini:1072 response → gemini:1072 prompt | Gemini web (lineage), thread th_a234a9df, 2026-01-25; copied: 1 pasted, 0 lifted into archive notes |
| gemini-1073-prompt | gemini:1073 response → gemini:1073 prompt | Gemini web (lineage), thread th_a234a9df, 2026-01-25; not copied |
| gemini-1074-prompt | gemini:1074 response → gemini:1074 prompt | Gemini web (lineage), thread th_a234a9df, 2026-01-25; not copied |
| gemini-3010-prompt | gemini:3010 response → gemini:3010 prompt | Gemini web (lineage), thread th_a23d55cf, 2026-03-28; not copied |
| gemini-347-prompt | gemini:347 response → gemini:347 prompt | Gemini web (lineage), thread th_a2e41ba7, 2025-12-08; not copied |
| gemini-348-prompt | gemini:348 response → gemini:348 prompt | Gemini web (lineage), thread th_a2e41ba7, 2025-12-08; not copied |
| gemini-349-prompt | gemini:349 response → gemini:349 prompt | Gemini web (lineage), thread th_a2e41ba7, 2025-12-08; not copied |
| gemini-350-prompt | gemini:350 response → gemini:350 prompt | Gemini web (lineage), thread th_a2e41ba7, 2025-12-08; not copied |
| gemini-351-prompt | gemini:351 response → gemini:351 prompt | Gemini web (lineage), thread th_a2e41ba7, 2025-12-08; not copied |
| gemini-352-prompt | gemini:352 response → gemini:352 prompt | Gemini web (lineage), thread th_a2e41ba7, 2025-12-08; not copied |
| gemini-353-prompt | gemini:353 response → gemini:353 prompt | Gemini web (lineage), thread th_a2e41ba7, 2025-12-08; not copied |
| gemini-3241-prompt | gemini:3241 response → gemini:3241 prompt | Gemini web (lineage), thread th_a3284b4b, 2026-04-19; not copied |
| gemini-3242-prompt | gemini:3242 response → gemini:3242 prompt | Gemini web (lineage), thread th_a3284b4b, 2026-04-19; not copied |
| gemini-3158-prompt | gemini:3158 response → gemini:3158 prompt | Gemini web (lineage), thread th_a40774e6, 2026-04-12; not copied |
| gemini-3159-prompt | gemini:3159 response → gemini:3159 prompt | Gemini web (lineage), thread th_a40774e6, 2026-04-12; not copied |
| gemini-1553-prompt | gemini:1553 response → gemini:1553 prompt | Gemini web (lineage), thread th_a49ea5bb, 2026-02-09; not copied |
| gemini-1554-prompt | gemini:1554 response → gemini:1554 prompt | Gemini web (lineage), thread th_a49ea5bb, 2026-02-09; copied: 5 pasted, 0 lifted into archive notes |
| gemini-1555-prompt | gemini:1555 response → gemini:1555 prompt | Gemini web (lineage), thread th_a49ea5bb, 2026-02-09; not copied |
| gemini-1556-prompt | gemini:1556 response → gemini:1556 prompt | Gemini web (lineage), thread th_a49ea5bb, 2026-02-09; not copied |
| gemini-1557-prompt | gemini:1557 response → gemini:1557 prompt | Gemini web (lineage), thread th_a49ea5bb, 2026-02-09; not copied |
| gemini-1558-prompt | gemini:1558 response → gemini:1558 prompt | Gemini web (lineage), thread th_a49ea5bb, 2026-02-09; not copied |
| gemini-2866-prompt | gemini:2866 response → gemini:2866 prompt | Gemini web (lineage), thread th_a5a47402, 2026-03-23; copied: 3 pasted, 0 lifted into archive notes |
| gemini-569-prompt | gemini:569 response → gemini:569 prompt | Gemini web (lineage), thread th_a5b56ac0, 2026-01-09; not copied |
| gemini-570-prompt | gemini:570 response → gemini:570 prompt | Gemini web (lineage), thread th_a5b56ac0, 2026-01-09; not copied |
| gemini-571-prompt | gemini:571 response → gemini:571 prompt | Gemini web (lineage), thread th_a5b56ac0, 2026-01-09; not copied |
| gemini-2521-prompt | gemini:2521 response → gemini:2521 prompt | Gemini web (lineage), thread th_a65164db, 2026-03-08; not copied |
| gemini-2522-prompt | gemini:2522 response → gemini:2522 prompt | Gemini web (lineage), thread th_a65164db, 2026-03-08; not copied |
| gemini-103-prompt | gemini:103 response → gemini:103 prompt | Gemini web (lineage), thread th_a66842ce, 2025-12-02; not copied |
| gemini-1528-prompt | gemini:1528 response → gemini:1528 prompt | Gemini web (lineage), thread th_a68d392d, 2026-02-08; not copied |
| gemini-1529-prompt | gemini:1529 response → gemini:1529 prompt | Gemini web (lineage), thread th_a68d392d, 2026-02-08; not copied |
| gemini-1530-prompt | gemini:1530 response → gemini:1530 prompt | Gemini web (lineage), thread th_a68d392d, 2026-02-08; not copied |
| gemini-1531-prompt | gemini:1531 response → gemini:1531 prompt | Gemini web (lineage), thread th_a68d392d, 2026-02-08; not copied |
| gemini-1532-prompt | gemini:1532 response → gemini:1532 prompt | Gemini web (lineage), thread th_a68d392d, 2026-02-08; not copied |
| gemini-2370-prompt | gemini:2370 response → gemini:2370 prompt | Gemini web (lineage), thread th_a697f7c0, 2026-03-04; copied: 0 pasted, 1 lifted into archive notes |
| gemini-2371-prompt | gemini:2371 response → gemini:2371 prompt | Gemini web (lineage), thread th_a697f7c0, 2026-03-04; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2372-prompt | gemini:2372 response → gemini:2372 prompt | Gemini web (lineage), thread th_a697f7c0, 2026-03-04; not copied |
| gemini-2373-prompt | gemini:2373 response → gemini:2373 prompt | Gemini web (lineage), thread th_a697f7c0, 2026-03-04; not copied |
| gemini-2916-prompt | gemini:2916 response → gemini:2916 prompt | Gemini web (lineage), thread th_a7001e89, 2026-03-24; copied: 1 pasted, 2 lifted into archive notes |
| gemini-2649-prompt | gemini:2649 response → gemini:2649 prompt | Gemini web (lineage), thread th_a70b232e, 2026-03-10; copied: 0 pasted, 1 lifted into archive notes |
| gemini-2650-prompt | gemini:2650 response → gemini:2650 prompt | Gemini web (lineage), thread th_a70b232e, 2026-03-10; not copied |
| gemini-2651-prompt | gemini:2651 response → gemini:2651 prompt | Gemini web (lineage), thread th_a70b232e, 2026-03-10; not copied |
| gemini-1973-prompt | gemini:1973 response → gemini:1973 prompt | Gemini web (lineage), thread th_a76c8a76, 2026-02-20; copied: 0 pasted, 1 lifted into archive notes |
| gemini-1347-prompt | gemini:1347 response → gemini:1347 prompt | Gemini web (lineage), thread th_a77a879f, 2026-02-04; copied: 1 pasted, 0 lifted into archive notes |
| gemini-1348-prompt | gemini:1348 response → gemini:1348 prompt | Gemini web (lineage), thread th_a77a879f, 2026-02-04; copied: 1 pasted, 1 lifted into archive notes |
| gemini-1349-prompt | gemini:1349 response → gemini:1349 prompt | Gemini web (lineage), thread th_a77a879f, 2026-02-04; copied: 2 pasted, 1 lifted into archive notes |
| gemini-1350-prompt | gemini:1350 response → gemini:1350 prompt | Gemini web (lineage), thread th_a77a879f, 2026-02-04; not copied |
| gemini-1351-prompt | gemini:1351 response → gemini:1351 prompt | Gemini web (lineage), thread th_a77a879f, 2026-02-04; copied: 6 pasted, 0 lifted into archive notes |
| gemini-1352-prompt | gemini:1352 response → gemini:1352 prompt | Gemini web (lineage), thread th_a77a879f, 2026-02-04; copied: 1 pasted, 1 lifted into archive notes |
| gemini-1353-prompt | gemini:1353 response → gemini:1353 prompt | Gemini web (lineage), thread th_a77a879f, 2026-02-04; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2567-prompt | gemini:2567 response → gemini:2567 prompt | Gemini web (lineage), thread th_a87f514c, 2026-03-09; not copied |
| gemini-2568-prompt | gemini:2568 response → gemini:2568 prompt | Gemini web (lineage), thread th_a87f514c, 2026-03-09; not copied |
| gemini-2569-prompt | gemini:2569 response → gemini:2569 prompt | Gemini web (lineage), thread th_a87f514c, 2026-03-09; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2570-prompt | gemini:2570 response → gemini:2570 prompt | Gemini web (lineage), thread th_a87f514c, 2026-03-09; not copied |
| gemini-2571-prompt | gemini:2571 response → gemini:2571 prompt | Gemini web (lineage), thread th_a87f514c, 2026-03-09; copied: 2 pasted, 1 lifted into archive notes |
| gemini-2572-prompt | gemini:2572 response → gemini:2572 prompt | Gemini web (lineage), thread th_a87f514c, 2026-03-09; copied: 1 pasted, 0 lifted into archive notes |
| gemini-1586-prompt | gemini:1586 response → gemini:1586 prompt | Gemini web (lineage), thread th_a97eb67c, 2026-02-09 to 2026-02-10; not copied |
| gemini-1587-prompt | gemini:1587 response → gemini:1587 prompt | Gemini web (lineage), thread th_a97eb67c, 2026-02-09 to 2026-02-10; not copied |
| gemini-1588-prompt | gemini:1588 response → gemini:1588 prompt | Gemini web (lineage), thread th_a97eb67c, 2026-02-09 to 2026-02-10; not copied |
| gemini-1589-prompt | gemini:1589 response → gemini:1589 prompt | Gemini web (lineage), thread th_a97eb67c, 2026-02-09 to 2026-02-10; not copied |
| gemini-1590-prompt | gemini:1590 response → gemini:1590 prompt | Gemini web (lineage), thread th_a97eb67c, 2026-02-09 to 2026-02-10; not copied |
| gemini-1591-prompt | gemini:1591 response → gemini:1591 prompt | Gemini web (lineage), thread th_a97eb67c, 2026-02-09 to 2026-02-10; copied: 0 pasted, 1 lifted into archive notes |
| gemini-1592-prompt | gemini:1592 response → gemini:1592 prompt | Gemini web (lineage), thread th_a97eb67c, 2026-02-09 to 2026-02-10; not copied |
| gemini-1593-prompt | gemini:1593 response → gemini:1593 prompt | Gemini web (lineage), thread th_a97eb67c, 2026-02-09 to 2026-02-10; not copied |
| gemini-1594-prompt | gemini:1594 response → gemini:1594 prompt | Gemini web (lineage), thread th_a97eb67c, 2026-02-09 to 2026-02-10; not copied |
| gemini-1595-prompt | gemini:1595 response → gemini:1595 prompt | Gemini web (lineage), thread th_a97eb67c, 2026-02-09 to 2026-02-10; not copied |
| gemini-2503-prompt | gemini:2503 response → gemini:2503 prompt | Gemini web (lineage), thread th_a98aa013, 2026-03-06; not copied |
| gemini-2504-prompt | gemini:2504 response → gemini:2504 prompt | Gemini web (lineage), thread th_a98aa013, 2026-03-06; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2505-prompt | gemini:2505 response → gemini:2505 prompt | Gemini web (lineage), thread th_a98aa013, 2026-03-06; not copied |
| gemini-2506-prompt | gemini:2506 response → gemini:2506 prompt | Gemini web (lineage), thread th_a98aa013, 2026-03-06; not copied |
| gemini-2507-prompt | gemini:2507 response → gemini:2507 prompt | Gemini web (lineage), thread th_a98aa013, 2026-03-06; not copied |
| gemini-2508-prompt | gemini:2508 response → gemini:2508 prompt | Gemini web (lineage), thread th_a98aa013, 2026-03-06; not copied |
| gemini-1306-prompt | gemini:1306 response → gemini:1306 prompt | Gemini web (lineage), thread th_ab4faecb, 2026-02-01; not copied |
| gemini-251-prompt | gemini:251 response → gemini:251 prompt | Gemini web (lineage), thread th_ac6a25ac, 2025-12-06; not copied |
| gemini-2709-prompt | gemini:2709 response → gemini:2709 prompt | Gemini web (lineage), thread th_ad74743b, 2026-03-16; not copied |
| gemini-2710-prompt | gemini:2710 response → gemini:2710 prompt | Gemini web (lineage), thread th_ad74743b, 2026-03-16; not copied |
| gemini-2711-prompt | gemini:2711 response → gemini:2711 prompt | Gemini web (lineage), thread th_ad74743b, 2026-03-16; not copied |
| gemini-2712-prompt | gemini:2712 response → gemini:2712 prompt | Gemini web (lineage), thread th_ad74743b, 2026-03-16; not copied |
| gemini-2713-prompt | gemini:2713 response → gemini:2713 prompt | Gemini web (lineage), thread th_ad74743b, 2026-03-16; not copied |
| gemini-2714-prompt | gemini:2714 response → gemini:2714 prompt | Gemini web (lineage), thread th_ad74743b, 2026-03-16; not copied |
| gemini-2715-prompt | gemini:2715 response → gemini:2715 prompt | Gemini web (lineage), thread th_ad74743b, 2026-03-16; not copied |
| gemini-2716-prompt | gemini:2716 response → gemini:2716 prompt | Gemini web (lineage), thread th_ad74743b, 2026-03-16; not copied |
| gemini-2717-prompt | gemini:2717 response → gemini:2717 prompt | Gemini web (lineage), thread th_ad74743b, 2026-03-16; not copied |
| gemini-2718-prompt | gemini:2718 response → gemini:2718 prompt | Gemini web (lineage), thread th_ad74743b, 2026-03-16; not copied |
| gemini-2719-prompt | gemini:2719 response → gemini:2719 prompt | Gemini web (lineage), thread th_ad74743b, 2026-03-16; not copied |
| gemini-2720-prompt | gemini:2720 response → gemini:2720 prompt | Gemini web (lineage), thread th_ad74743b, 2026-03-16; not copied |
| gemini-2721-prompt | gemini:2721 response → gemini:2721 prompt | Gemini web (lineage), thread th_ad74743b, 2026-03-16; not copied |
| gemini-2722-prompt | gemini:2722 response → gemini:2722 prompt | Gemini web (lineage), thread th_ad74743b, 2026-03-16; not copied |
| gemini-2723-prompt | gemini:2723 response → gemini:2723 prompt | Gemini web (lineage), thread th_ad74743b, 2026-03-16; not copied |
| gemini-1984-prompt | gemini:1984 response → gemini:1984 prompt | Gemini web (lineage), thread th_ad8c4bf3, 2026-02-20; not copied |
| gemini-1985-prompt | gemini:1985 response → gemini:1985 prompt | Gemini web (lineage), thread th_ad8c4bf3, 2026-02-20; not copied |
| gemini-1986-prompt | gemini:1986 response → gemini:1986 prompt | Gemini web (lineage), thread th_ad8c4bf3, 2026-02-20; not copied |
| gemini-1987-prompt | gemini:1987 response → gemini:1987 prompt | Gemini web (lineage), thread th_ad8c4bf3, 2026-02-20; not copied |
| gemini-3249-prompt | gemini:3249 response → gemini:3249 prompt | Gemini web (lineage), thread th_ade32872, 2026-04-23; not copied |
| gemini-2896-prompt | gemini:2896 response → gemini:2896 prompt | Gemini web (lineage), thread th_adf9a17a, 2026-03-23; not copied |
| gemini-2897-prompt | gemini:2897 response → gemini:2897 prompt | Gemini web (lineage), thread th_adf9a17a, 2026-03-23; not copied |
| gemini-2898-prompt | gemini:2898 response → gemini:2898 prompt | Gemini web (lineage), thread th_adf9a17a, 2026-03-23; not copied |
| gemini-2899-prompt | gemini:2899 response → gemini:2899 prompt | Gemini web (lineage), thread th_adf9a17a, 2026-03-23; not copied |
| gemini-2900-prompt | gemini:2900 response → gemini:2900 prompt | Gemini web (lineage), thread th_adf9a17a, 2026-03-23; not copied |
| gemini-1772-prompt | gemini:1772 response → gemini:1772 prompt | Gemini web (lineage), thread th_ae52405c, 2026-02-12; copied: 0 pasted, 1 lifted into archive notes |
| gemini-2724-prompt | gemini:2724 response → gemini:2724 prompt | Gemini web (lineage), thread th_ae59ad55, 2026-03-17; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2725-prompt | gemini:2725 response → gemini:2725 prompt | Gemini web (lineage), thread th_ae59ad55, 2026-03-17; not copied |
| gemini-2726-prompt | gemini:2726 response → gemini:2726 prompt | Gemini web (lineage), thread th_ae59ad55, 2026-03-17; not copied |
| gemini-2727-prompt | gemini:2727 response → gemini:2727 prompt | Gemini web (lineage), thread th_ae59ad55, 2026-03-17; not copied |
| gemini-2728-prompt | gemini:2728 response → gemini:2728 prompt | Gemini web (lineage), thread th_ae59ad55, 2026-03-17; not copied |
| gemini-619-prompt | gemini:619 response → gemini:619 prompt | Gemini web (lineage), thread th_ae6cdb68, 2026-01-10; not copied |
| gemini-2611-prompt | gemini:2611 response → gemini:2611 prompt | Gemini web (lineage), thread th_aefda15e, 2026-03-09; not copied |
| gemini-2612-prompt | gemini:2612 response → gemini:2612 prompt | Gemini web (lineage), thread th_aefda15e, 2026-03-09; not copied |
| gemini-1970-prompt | gemini:1970 response → gemini:1970 prompt | Gemini web (lineage), thread th_af2bbe11, 2026-02-20; not copied |
| gemini-1971-prompt | gemini:1971 response → gemini:1971 prompt | Gemini web (lineage), thread th_af2bbe11, 2026-02-20; copied: 0 pasted, 1 lifted into archive notes |
| gemini-1972-prompt | gemini:1972 response → gemini:1972 prompt | Gemini web (lineage), thread th_af2bbe11, 2026-02-20; not copied |
| gemini-215-prompt | gemini:215 response → gemini:215 prompt | Gemini web (lineage), thread th_af36eeed, 2025-12-05; not copied |
| gemini-216-prompt | gemini:216 response → gemini:216 prompt | Gemini web (lineage), thread th_af36eeed, 2025-12-05; not copied |
| gemini-217-prompt | gemini:217 response → gemini:217 prompt | Gemini web (lineage), thread th_af36eeed, 2025-12-05; not copied |
| gemini-218-prompt | gemini:218 response → gemini:218 prompt | Gemini web (lineage), thread th_af36eeed, 2025-12-05; not copied |
| gemini-2770-prompt | gemini:2770 response → gemini:2770 prompt | Gemini web (lineage), thread th_af7722de, 2026-03-20; not copied |
| gemini-2771-prompt | gemini:2771 response → gemini:2771 prompt | Gemini web (lineage), thread th_af7722de, 2026-03-20; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2772-prompt | gemini:2772 response → gemini:2772 prompt | Gemini web (lineage), thread th_af7722de, 2026-03-20; not copied |
| gemini-3167-prompt | gemini:3167 response → gemini:3167 prompt | Gemini web (lineage), thread th_b010d1de, 2026-04-15; not copied |
| gemini-3168-prompt | gemini:3168 response → gemini:3168 prompt | Gemini web (lineage), thread th_b010d1de, 2026-04-15; not copied |
| gemini-3169-prompt | gemini:3169 response → gemini:3169 prompt | Gemini web (lineage), thread th_b010d1de, 2026-04-15; not copied |
| gemini-3170-prompt | gemini:3170 response → gemini:3170 prompt | Gemini web (lineage), thread th_b010d1de, 2026-04-15; not copied |
| gemini-3171-prompt | gemini:3171 response → gemini:3171 prompt | Gemini web (lineage), thread th_b010d1de, 2026-04-15; not copied |
| gemini-3172-prompt | gemini:3172 response → gemini:3172 prompt | Gemini web (lineage), thread th_b010d1de, 2026-04-15; not copied |
| gemini-3173-prompt | gemini:3173 response → gemini:3173 prompt | Gemini web (lineage), thread th_b010d1de, 2026-04-15; not copied |
| gemini-63-prompt | gemini:63 response → gemini:63 prompt | Gemini web (lineage), thread th_b07e4bd0, 2025-11-26; not copied |
| gemini-64-prompt | gemini:64 response → gemini:64 prompt | Gemini web (lineage), thread th_b07e4bd0, 2025-11-26; not copied |
| gemini-335-prompt | gemini:335 response → gemini:335 prompt | Gemini web (lineage), thread th_b0bca611, 2025-12-08; not copied |
| gemini-336-prompt | gemini:336 response → gemini:336 prompt | Gemini web (lineage), thread th_b0bca611, 2025-12-08; copied: 0 pasted, 1 lifted into archive notes |
| gemini-337-prompt | gemini:337 response → gemini:337 prompt | Gemini web (lineage), thread th_b0bca611, 2025-12-08; not copied |
| gemini-338-prompt | gemini:338 response → gemini:338 prompt | Gemini web (lineage), thread th_b0bca611, 2025-12-08; copied: 2 pasted, 0 lifted into archive notes |
| gemini-339-prompt | gemini:339 response → gemini:339 prompt | Gemini web (lineage), thread th_b0bca611, 2025-12-08; copied: 1 pasted, 1 lifted into archive notes |
| gemini-1576-prompt | gemini:1576 response → gemini:1576 prompt | Gemini web (lineage), thread th_b0cc8fe5, 2026-02-09; not copied |
| gemini-1577-prompt | gemini:1577 response → gemini:1577 prompt | Gemini web (lineage), thread th_b0cc8fe5, 2026-02-09; not copied |
| gemini-1578-prompt | gemini:1578 response → gemini:1578 prompt | Gemini web (lineage), thread th_b0cc8fe5, 2026-02-09; copied: 2 pasted, 0 lifted into archive notes |
| gemini-1579-prompt | gemini:1579 response → gemini:1579 prompt | Gemini web (lineage), thread th_b0cc8fe5, 2026-02-09; not copied |
| gemini-1580-prompt | gemini:1580 response → gemini:1580 prompt | Gemini web (lineage), thread th_b0cc8fe5, 2026-02-09; not copied |
| gemini-1581-prompt | gemini:1581 response → gemini:1581 prompt | Gemini web (lineage), thread th_b0cc8fe5, 2026-02-09; not copied |
| gemini-1582-prompt | gemini:1582 response → gemini:1582 prompt | Gemini web (lineage), thread th_b0cc8fe5, 2026-02-09; not copied |
| gemini-1583-prompt | gemini:1583 response → gemini:1583 prompt | Gemini web (lineage), thread th_b0cc8fe5, 2026-02-09; copied: 0 pasted, 1 lifted into archive notes |
| gemini-1584-prompt | gemini:1584 response → gemini:1584 prompt | Gemini web (lineage), thread th_b0cc8fe5, 2026-02-09; not copied |
| gemini-1585-prompt | gemini:1585 response → gemini:1585 prompt | Gemini web (lineage), thread th_b0cc8fe5, 2026-02-09; copied: 0 pasted, 1 lifted into archive notes |
| gemini-1362-prompt | gemini:1362 response → gemini:1362 prompt | Gemini web (lineage), thread th_b1218d07, 2026-02-04; not copied |
| gemini-2632-prompt | gemini:2632 response → gemini:2632 prompt | Gemini web (lineage), thread th_b12392df, 2026-03-10; not copied |
| gemini-2633-prompt | gemini:2633 response → gemini:2633 prompt | Gemini web (lineage), thread th_b12392df, 2026-03-10; not copied |
| gemini-1835-prompt | gemini:1835 response → gemini:1835 prompt | Gemini web (lineage), thread th_b16dbdce, 2026-02-14; not copied |
| gemini-1836-prompt | gemini:1836 response → gemini:1836 prompt | Gemini web (lineage), thread th_b16dbdce, 2026-02-14; not copied |
| gemini-1837-prompt | gemini:1837 response → gemini:1837 prompt | Gemini web (lineage), thread th_b16dbdce, 2026-02-14; not copied |
| gemini-1838-prompt | gemini:1838 response → gemini:1838 prompt | Gemini web (lineage), thread th_b16dbdce, 2026-02-14; not copied |
| gemini-1839-prompt | gemini:1839 response → gemini:1839 prompt | Gemini web (lineage), thread th_b16dbdce, 2026-02-14; not copied |
| gemini-1840-prompt | gemini:1840 response → gemini:1840 prompt | Gemini web (lineage), thread th_b16dbdce, 2026-02-14; not copied |
| gemini-1255-prompt | gemini:1255 response → gemini:1255 prompt | Gemini web (lineage), thread th_b16fd530, 2026-01-31; not copied |
| gemini-1256-prompt | gemini:1256 response → gemini:1256 prompt | Gemini web (lineage), thread th_b16fd530, 2026-01-31; not copied |
| gemini-2834-prompt | gemini:2834 response → gemini:2834 prompt | Gemini web (lineage), thread th_b178a132, 2026-03-22; not copied |
| gemini-1100-prompt | gemini:1100 response → gemini:1100 prompt | Gemini web (lineage), thread th_b1932164, 2026-01-26; not copied |
| gemini-1730-prompt | gemini:1730 response → gemini:1730 prompt | Gemini web (lineage), thread th_b197c7c4, 2026-02-11; not copied |
| gemini-456-prompt | gemini:456 response → gemini:456 prompt | Gemini web (lineage), thread th_b1c3a9b2, 2026-01-08; not copied |
| gemini-3116-prompt | gemini:3116 response → gemini:3116 prompt | Gemini web (lineage), thread th_b1d35c0f, 2026-04-08; not copied |
| gemini-3117-prompt | gemini:3117 response → gemini:3117 prompt | Gemini web (lineage), thread th_b1d35c0f, 2026-04-08; not copied |
| gemini-3118-prompt | gemini:3118 response → gemini:3118 prompt | Gemini web (lineage), thread th_b1d35c0f, 2026-04-08; not copied |
| gemini-93-prompt | gemini:93 response → gemini:93 prompt | Gemini web (lineage), thread th_b2146aa6, 2025-12-02; copied: 2 pasted, 0 lifted into archive notes |
| gemini-65-prompt | gemini:65 response → gemini:65 prompt | Gemini web (lineage), thread th_b29dc976, 2025-11-26; not copied |
| gemini-2652-prompt | gemini:2652 response → gemini:2652 prompt | Gemini web (lineage), thread th_b2b78a38, 2026-03-10; not copied |
| gemini-2653-prompt | gemini:2653 response → gemini:2653 prompt | Gemini web (lineage), thread th_b2b78a38, 2026-03-10; not copied |
| gemini-2654-prompt | gemini:2654 response → gemini:2654 prompt | Gemini web (lineage), thread th_b2b78a38, 2026-03-10; not copied |
| gemini-2655-prompt | gemini:2655 response → gemini:2655 prompt | Gemini web (lineage), thread th_b2b78a38, 2026-03-10; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2656-prompt | gemini:2656 response → gemini:2656 prompt | Gemini web (lineage), thread th_b2b78a38, 2026-03-10; not copied |
| gemini-2657-prompt | gemini:2657 response → gemini:2657 prompt | Gemini web (lineage), thread th_b2b78a38, 2026-03-10; not copied |
| gemini-1124-prompt | gemini:1124 response → gemini:1124 prompt | Gemini web (lineage), thread th_b2b9e121, 2026-01-26; copied: 2 pasted, 0 lifted into archive notes |
| gemini-1125-prompt | gemini:1125 response → gemini:1125 prompt | Gemini web (lineage), thread th_b2b9e121, 2026-01-26; copied: 2 pasted, 0 lifted into archive notes |
| gemini-1126-prompt | gemini:1126 response → gemini:1126 prompt | Gemini web (lineage), thread th_b2b9e121, 2026-01-26; copied: 2 pasted, 0 lifted into archive notes |
| gemini-1127-prompt | gemini:1127 response → gemini:1127 prompt | Gemini web (lineage), thread th_b2b9e121, 2026-01-26; not copied |
| gemini-1128-prompt | gemini:1128 response → gemini:1128 prompt | Gemini web (lineage), thread th_b2b9e121, 2026-01-26; copied: 1 pasted, 1 lifted into archive notes |
| gemini-1129-prompt | gemini:1129 response → gemini:1129 prompt | Gemini web (lineage), thread th_b2b9e121, 2026-01-26; copied: 2 pasted, 0 lifted into archive notes |
| gemini-1130-prompt | gemini:1130 response → gemini:1130 prompt | Gemini web (lineage), thread th_b2b9e121, 2026-01-26; not copied |
| gemini-109-prompt | gemini:109 response → gemini:109 prompt | Gemini web (lineage), thread th_b2bef5d1, 2025-12-02; copied: 2 pasted, 0 lifted into archive notes |
| gemini-669-prompt | gemini:669 response → gemini:669 prompt | Gemini web (lineage), thread th_b309f7f0, 2026-01-12; not copied |
| gemini-324-prompt | gemini:324 response → gemini:324 prompt | Gemini web (lineage), thread th_b3126afe, 2025-12-08; not copied |
| gemini-325-prompt | gemini:325 response → gemini:325 prompt | Gemini web (lineage), thread th_b3126afe, 2025-12-08; not copied |
| gemini-326-prompt | gemini:326 response → gemini:326 prompt | Gemini web (lineage), thread th_b3126afe, 2025-12-08; not copied |
| gemini-574-prompt | gemini:574 response → gemini:574 prompt | Gemini web (lineage), thread th_b392b115, 2026-01-09; not copied |
| gemini-575-prompt | gemini:575 response → gemini:575 prompt | Gemini web (lineage), thread th_b392b115, 2026-01-09; copied: 1 pasted, 0 lifted into archive notes |
| gemini-576-prompt | gemini:576 response → gemini:576 prompt | Gemini web (lineage), thread th_b392b115, 2026-01-09; not copied |
| gemini-577-prompt | gemini:577 response → gemini:577 prompt | Gemini web (lineage), thread th_b392b115, 2026-01-09; not copied |
| gemini-323-prompt | gemini:323 response → gemini:323 prompt | Gemini web (lineage), thread th_b3a432a1, 2025-12-08; copied: 3 pasted, 0 lifted into archive notes |
| gemini-1308-prompt | gemini:1308 response → gemini:1308 prompt | Gemini web (lineage), thread th_b42cc6ac, 2026-02-02; not copied |
| gemini-1309-prompt | gemini:1309 response → gemini:1309 prompt | Gemini web (lineage), thread th_b42cc6ac, 2026-02-02; not copied |
| gemini-1310-prompt | gemini:1310 response → gemini:1310 prompt | Gemini web (lineage), thread th_b42cc6ac, 2026-02-02; not copied |
| gemini-1268-prompt | gemini:1268 response → gemini:1268 prompt | Gemini web (lineage), thread th_b4503503, 2026-02-01; not copied |
| gemini-2204-prompt | gemini:2204 response → gemini:2204 prompt | Gemini web (lineage), thread th_b4e18745, 2026-02-27; not copied |
| gemini-2205-prompt | gemini:2205 response → gemini:2205 prompt | Gemini web (lineage), thread th_b4e18745, 2026-02-27; not copied |
| gemini-354-prompt | gemini:354 response → gemini:354 prompt | Gemini web (lineage), thread th_b526376e, 2025-12-08 to 2025-12-09; not copied |
| gemini-355-prompt | gemini:355 response → gemini:355 prompt | Gemini web (lineage), thread th_b526376e, 2025-12-08 to 2025-12-09; not copied |
| gemini-356-prompt | gemini:356 response → gemini:356 prompt | Gemini web (lineage), thread th_b526376e, 2025-12-08 to 2025-12-09; not copied |
| gemini-357-prompt | gemini:357 response → gemini:357 prompt | Gemini web (lineage), thread th_b526376e, 2025-12-08 to 2025-12-09; not copied |
| gemini-358-prompt | gemini:358 response → gemini:358 prompt | Gemini web (lineage), thread th_b526376e, 2025-12-08 to 2025-12-09; not copied |
| gemini-359-prompt | gemini:359 response → gemini:359 prompt | Gemini web (lineage), thread th_b526376e, 2025-12-08 to 2025-12-09; not copied |
| gemini-360-prompt | gemini:360 response → gemini:360 prompt | Gemini web (lineage), thread th_b526376e, 2025-12-08 to 2025-12-09; not copied |
| gemini-361-prompt | gemini:361 response → gemini:361 prompt | Gemini web (lineage), thread th_b526376e, 2025-12-08 to 2025-12-09; not copied |
| gemini-362-prompt | gemini:362 response → gemini:362 prompt | Gemini web (lineage), thread th_b526376e, 2025-12-08 to 2025-12-09; not copied |
| gemini-363-prompt | gemini:363 response → gemini:363 prompt | Gemini web (lineage), thread th_b526376e, 2025-12-08 to 2025-12-09; not copied |
| gemini-364-prompt | gemini:364 response → gemini:364 prompt | Gemini web (lineage), thread th_b526376e, 2025-12-08 to 2025-12-09; not copied |
| gemini-365-prompt | gemini:365 response → gemini:365 prompt | Gemini web (lineage), thread th_b526376e, 2025-12-08 to 2025-12-09; not copied |
| gemini-366-prompt | gemini:366 response → gemini:366 prompt | Gemini web (lineage), thread th_b526376e, 2025-12-08 to 2025-12-09; not copied |
| gemini-2630-prompt | gemini:2630 response → gemini:2630 prompt | Gemini web (lineage), thread th_b5c772ea, 2026-03-10; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2631-prompt | gemini:2631 response → gemini:2631 prompt | Gemini web (lineage), thread th_b5c772ea, 2026-03-10; not copied |
| gemini-2758-prompt | gemini:2758 response → gemini:2758 prompt | Gemini web (lineage), thread th_b6120dc1, 2026-03-19; not copied |
| gemini-2759-prompt | gemini:2759 response → gemini:2759 prompt | Gemini web (lineage), thread th_b6120dc1, 2026-03-19; not copied |
| gemini-2760-prompt | gemini:2760 response → gemini:2760 prompt | Gemini web (lineage), thread th_b6120dc1, 2026-03-19; not copied |
| gemini-2761-prompt | gemini:2761 response → gemini:2761 prompt | Gemini web (lineage), thread th_b6120dc1, 2026-03-19; not copied |
| gemini-2762-prompt | gemini:2762 response → gemini:2762 prompt | Gemini web (lineage), thread th_b6120dc1, 2026-03-19; not copied |
| gemini-2763-prompt | gemini:2763 response → gemini:2763 prompt | Gemini web (lineage), thread th_b6120dc1, 2026-03-19; not copied |
| gemini-2764-prompt | gemini:2764 response → gemini:2764 prompt | Gemini web (lineage), thread th_b6120dc1, 2026-03-19; not copied |
| gemini-2765-prompt | gemini:2765 response → gemini:2765 prompt | Gemini web (lineage), thread th_b6120dc1, 2026-03-19; not copied |
| gemini-2766-prompt | gemini:2766 response → gemini:2766 prompt | Gemini web (lineage), thread th_b6120dc1, 2026-03-19; not copied |
| gemini-2767-prompt | gemini:2767 response → gemini:2767 prompt | Gemini web (lineage), thread th_b6120dc1, 2026-03-19; not copied |
| gemini-2768-prompt | gemini:2768 response → gemini:2768 prompt | Gemini web (lineage), thread th_b6120dc1, 2026-03-19; not copied |
| gemini-2769-prompt | gemini:2769 response → gemini:2769 prompt | Gemini web (lineage), thread th_b6120dc1, 2026-03-19; not copied |
| gemini-426-prompt | gemini:426 response → gemini:426 prompt | Gemini web (lineage), thread th_b6715c1c, 2025-12-28; not copied |
| gemini-1968-prompt | gemini:1968 response → gemini:1968 prompt | Gemini web (lineage), thread th_b68db30e, 2026-02-20; not copied |
| gemini-1969-prompt | gemini:1969 response → gemini:1969 prompt | Gemini web (lineage), thread th_b68db30e, 2026-02-20; not copied |
| gemini-1121-prompt | gemini:1121 response → gemini:1121 prompt | Gemini web (lineage), thread th_b69127f9, 2026-01-26; copied: 2 pasted, 0 lifted into archive notes |
| gemini-1122-prompt | gemini:1122 response → gemini:1122 prompt | Gemini web (lineage), thread th_b69127f9, 2026-01-26; not copied |
| gemini-1123-prompt | gemini:1123 response → gemini:1123 prompt | Gemini web (lineage), thread th_b69127f9, 2026-01-26; copied: 2 pasted, 0 lifted into archive notes |
| gemini-133-prompt | gemini:133 response → gemini:133 prompt | Gemini web (lineage), thread th_b6b34993, 2025-12-03; copied: 2 pasted, 0 lifted into archive notes |
| gemini-134-prompt | gemini:134 response → gemini:134 prompt | Gemini web (lineage), thread th_b6b34993, 2025-12-03; copied: 3 pasted, 0 lifted into archive notes |
| gemini-135-prompt | gemini:135 response → gemini:135 prompt | Gemini web (lineage), thread th_b6b34993, 2025-12-03; copied: 3 pasted, 0 lifted into archive notes |
| gemini-136-prompt | gemini:136 response → gemini:136 prompt | Gemini web (lineage), thread th_b6b34993, 2025-12-03; not copied |
| gemini-137-prompt | gemini:137 response → gemini:137 prompt | Gemini web (lineage), thread th_b6b34993, 2025-12-03; copied: 1 pasted, 0 lifted into archive notes |
| gemini-138-prompt | gemini:138 response → gemini:138 prompt | Gemini web (lineage), thread th_b6b34993, 2025-12-03; not copied |
| gemini-645-prompt | gemini:645 response → gemini:645 prompt | Gemini web (lineage), thread th_b71516ee, 2026-01-11; not copied |
| gemini-2683-prompt | gemini:2683 response → gemini:2683 prompt | Gemini web (lineage), thread th_b736a029, 2026-03-13; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2684-prompt | gemini:2684 response → gemini:2684 prompt | Gemini web (lineage), thread th_b736a029, 2026-03-13; not copied |
| gemini-2685-prompt | gemini:2685 response → gemini:2685 prompt | Gemini web (lineage), thread th_b736a029, 2026-03-13; not copied |
| gemini-2686-prompt | gemini:2686 response → gemini:2686 prompt | Gemini web (lineage), thread th_b736a029, 2026-03-13; not copied |
| gemini-1567-prompt | gemini:1567 response → gemini:1567 prompt | Gemini web (lineage), thread th_b7598d1f, 2026-02-09; not copied |
| gemini-1568-prompt | gemini:1568 response → gemini:1568 prompt | Gemini web (lineage), thread th_b7598d1f, 2026-02-09; not copied |
| gemini-1569-prompt | gemini:1569 response → gemini:1569 prompt | Gemini web (lineage), thread th_b7598d1f, 2026-02-09; not copied |
| gemini-1570-prompt | gemini:1570 response → gemini:1570 prompt | Gemini web (lineage), thread th_b7598d1f, 2026-02-09; not copied |
| gemini-1571-prompt | gemini:1571 response → gemini:1571 prompt | Gemini web (lineage), thread th_b7598d1f, 2026-02-09; not copied |
| gemini-1572-prompt | gemini:1572 response → gemini:1572 prompt | Gemini web (lineage), thread th_b7598d1f, 2026-02-09; not copied |
| gemini-1573-prompt | gemini:1573 response → gemini:1573 prompt | Gemini web (lineage), thread th_b7598d1f, 2026-02-09; not copied |
| gemini-887-prompt | gemini:887 response → gemini:887 prompt | Gemini web (lineage), thread th_b809a6fb, 2026-01-22; not copied |
| gemini-888-prompt | gemini:888 response → gemini:888 prompt | Gemini web (lineage), thread th_b809a6fb, 2026-01-22; not copied |
| gemini-85-prompt | gemini:85 response → gemini:85 prompt | Gemini web (lineage), thread th_b86c6ac3, 2025-12-02; not copied |
| gemini-3236-prompt | gemini:3236 response → gemini:3236 prompt | Gemini web (lineage), thread th_b880156f, 2026-04-19; not copied |
| gemini-3237-prompt | gemini:3237 response → gemini:3237 prompt | Gemini web (lineage), thread th_b880156f, 2026-04-19; not copied |
| gemini-3238-prompt | gemini:3238 response → gemini:3238 prompt | Gemini web (lineage), thread th_b880156f, 2026-04-19; not copied |
| gemini-3239-prompt | gemini:3239 response → gemini:3239 prompt | Gemini web (lineage), thread th_b880156f, 2026-04-19; not copied |
| gemini-3240-prompt | gemini:3240 response → gemini:3240 prompt | Gemini web (lineage), thread th_b880156f, 2026-04-19; not copied |
| gemini-1316-prompt | gemini:1316 response → gemini:1316 prompt | Gemini web (lineage), thread th_b8e21008, 2026-02-03; not copied |
| gemini-1317-prompt | gemini:1317 response → gemini:1317 prompt | Gemini web (lineage), thread th_b8e21008, 2026-02-03; not copied |
| gemini-1318-prompt | gemini:1318 response → gemini:1318 prompt | Gemini web (lineage), thread th_b8e21008, 2026-02-03; not copied |
| gemini-2552-prompt | gemini:2552 response → gemini:2552 prompt | Gemini web (lineage), thread th_b94ee9f5, 2026-03-08; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2931-prompt | gemini:2931 response → gemini:2931 prompt | Gemini web (lineage), thread th_ba08d4cd, 2026-03-25; not copied |
| gemini-2499-prompt | gemini:2499 response → gemini:2499 prompt | Gemini web (lineage), thread th_ba4f827e, 2026-03-06; not copied |
| gemini-1241-prompt | gemini:1241 response → gemini:1241 prompt | Gemini web (lineage), thread th_ba5debad, 2026-01-30; not copied |
| gemini-1242-prompt | gemini:1242 response → gemini:1242 prompt | Gemini web (lineage), thread th_ba5debad, 2026-01-30; not copied |
| gemini-1243-prompt | gemini:1243 response → gemini:1243 prompt | Gemini web (lineage), thread th_ba5debad, 2026-01-30; not copied |
| gemini-1244-prompt | gemini:1244 response → gemini:1244 prompt | Gemini web (lineage), thread th_ba5debad, 2026-01-30; not copied |
| gemini-318-prompt | gemini:318 response → gemini:318 prompt | Gemini web (lineage), thread th_ba9c29a1, 2025-12-08; copied: 2 pasted, 0 lifted into archive notes |
| gemini-797-prompt | gemini:797 response → gemini:797 prompt | Gemini web (lineage), thread th_bb1b8d0a, 2026-01-18; not copied |
| gemini-798-prompt | gemini:798 response → gemini:798 prompt | Gemini web (lineage), thread th_bb1b8d0a, 2026-01-18; not copied |
| gemini-799-prompt | gemini:799 response → gemini:799 prompt | Gemini web (lineage), thread th_bb1b8d0a, 2026-01-18; not copied |
| gemini-800-prompt | gemini:800 response → gemini:800 prompt | Gemini web (lineage), thread th_bb1b8d0a, 2026-01-18; not copied |
| gemini-801-prompt | gemini:801 response → gemini:801 prompt | Gemini web (lineage), thread th_bb1b8d0a, 2026-01-18; not copied |
| gemini-503-prompt | gemini:503 response → gemini:503 prompt | Gemini web (lineage), thread th_bbba11a4, 2026-01-09; not copied |
| gemini-2774-prompt | gemini:2774 response → gemini:2774 prompt | Gemini web (lineage), thread th_bc5a4255, 2026-03-20; not copied |
| gemini-2775-prompt | gemini:2775 response → gemini:2775 prompt | Gemini web (lineage), thread th_bc5a4255, 2026-03-20; not copied |
| gemini-2776-prompt | gemini:2776 response → gemini:2776 prompt | Gemini web (lineage), thread th_bc5a4255, 2026-03-20; not copied |
| gemini-2777-prompt | gemini:2777 response → gemini:2777 prompt | Gemini web (lineage), thread th_bc5a4255, 2026-03-20; not copied |
| gemini-2778-prompt | gemini:2778 response → gemini:2778 prompt | Gemini web (lineage), thread th_bc5a4255, 2026-03-20; not copied |
| gemini-2779-prompt | gemini:2779 response → gemini:2779 prompt | Gemini web (lineage), thread th_bc5a4255, 2026-03-20; not copied |
| gemini-331-prompt | gemini:331 response → gemini:331 prompt | Gemini web (lineage), thread th_bc68a1e8, 2025-12-08; copied: 1 pasted, 0 lifted into archive notes |
| gemini-332-prompt | gemini:332 response → gemini:332 prompt | Gemini web (lineage), thread th_bc68a1e8, 2025-12-08; copied: 1 pasted, 0 lifted into archive notes |
| gemini-333-prompt | gemini:333 response → gemini:333 prompt | Gemini web (lineage), thread th_bc68a1e8, 2025-12-08; copied: 0 pasted, 1 lifted into archive notes |
| gemini-334-prompt | gemini:334 response → gemini:334 prompt | Gemini web (lineage), thread th_bc68a1e8, 2025-12-08; not copied |
| gemini-292-prompt | gemini:292 response → gemini:292 prompt | Gemini web (lineage), thread th_bcae67e9, 2025-12-08; not copied |
| gemini-2784-prompt | gemini:2784 response → gemini:2784 prompt | Gemini web (lineage), thread th_bceb693b, 2026-03-20; copied: 1 pasted, 0 lifted into archive notes |
| gemini-10-prompt | gemini:10 response → gemini:10 prompt | Gemini web (lineage), thread th_bcf2d6e6, 2025-09-12; not copied |
| gemini-2843-prompt | gemini:2843 response → gemini:2843 prompt | Gemini web (lineage), thread th_be18c655, 2026-03-22; not copied |
| gemini-2844-prompt | gemini:2844 response → gemini:2844 prompt | Gemini web (lineage), thread th_be18c655, 2026-03-22; not copied |
| gemini-2845-prompt | gemini:2845 response → gemini:2845 prompt | Gemini web (lineage), thread th_be18c655, 2026-03-22; not copied |
| gemini-3232-prompt | gemini:3232 response → gemini:3232 prompt | Gemini web (lineage), thread th_be46face, 2026-04-19; not copied |
| gemini-3233-prompt | gemini:3233 response → gemini:3233 prompt | Gemini web (lineage), thread th_be46face, 2026-04-19; not copied |
| gemini-139-prompt | gemini:139 response → gemini:139 prompt | Gemini web (lineage), thread th_be5607aa, 2025-12-03; copied: 9 pasted, 0 lifted into archive notes |
| gemini-401-prompt | gemini:401 response → gemini:401 prompt | Gemini web (lineage), thread th_be880e81, 2025-12-28; copied: 2 pasted, 0 lifted into archive notes |
| gemini-2696-prompt | gemini:2696 response → gemini:2696 prompt | Gemini web (lineage), thread th_bfc5558d, 2026-03-16; not copied |
| gemini-2697-prompt | gemini:2697 response → gemini:2697 prompt | Gemini web (lineage), thread th_bfc5558d, 2026-03-16; not copied |
| gemini-2698-prompt | gemini:2698 response → gemini:2698 prompt | Gemini web (lineage), thread th_bfc5558d, 2026-03-16; not copied |
| gemini-2699-prompt | gemini:2699 response → gemini:2699 prompt | Gemini web (lineage), thread th_bfc5558d, 2026-03-16; not copied |
| gemini-2700-prompt | gemini:2700 response → gemini:2700 prompt | Gemini web (lineage), thread th_bfc5558d, 2026-03-16; not copied |
| gemini-2701-prompt | gemini:2701 response → gemini:2701 prompt | Gemini web (lineage), thread th_bfc5558d, 2026-03-16; not copied |
| gemini-2702-prompt | gemini:2702 response → gemini:2702 prompt | Gemini web (lineage), thread th_bfc5558d, 2026-03-16; not copied |
| gemini-453-prompt | gemini:453 response → gemini:453 prompt | Gemini web (lineage), thread th_bff97f07, 2026-01-07; not copied |
| gemini-454-prompt | gemini:454 response → gemini:454 prompt | Gemini web (lineage), thread th_bff97f07, 2026-01-07; not copied |
| gemini-1786-prompt | gemini:1786 response → gemini:1786 prompt | Gemini web (lineage), thread th_bffcae7d, 2026-02-13; not copied |
| gemini-618-prompt | gemini:618 response → gemini:618 prompt | Gemini web (lineage), thread th_c0db19ff, 2026-01-10; not copied |
| gemini-1691-prompt | gemini:1691 response → gemini:1691 prompt | Gemini web (lineage), thread th_c0faffd1, 2026-02-10; not copied |
| gemini-1692-prompt | gemini:1692 response → gemini:1692 prompt | Gemini web (lineage), thread th_c0faffd1, 2026-02-10; not copied |
| gemini-1693-prompt | gemini:1693 response → gemini:1693 prompt | Gemini web (lineage), thread th_c0faffd1, 2026-02-10; not copied |
| gemini-230-prompt | gemini:230 response → gemini:230 prompt | Gemini web (lineage), thread th_c1eb08ff, 2025-12-05; not copied |
| gemini-183-prompt | gemini:183 response → gemini:183 prompt | Gemini web (lineage), thread th_c1fe2bbf, 2025-12-04; copied: 3 pasted, 0 lifted into archive notes |
| gemini-67-prompt | gemini:67 response → gemini:67 prompt | Gemini web (lineage), thread th_c2381b47, 2025-11-26; not copied |
| gemini-68-prompt | gemini:68 response → gemini:68 prompt | Gemini web (lineage), thread th_c2381b47, 2025-11-26; not copied |
| gemini-726-prompt | gemini:726 response → gemini:726 prompt | Gemini web (lineage), thread th_c26ead8e, 2026-01-13 to 2026-01-14; not copied |
| gemini-727-prompt | gemini:727 response → gemini:727 prompt | Gemini web (lineage), thread th_c26ead8e, 2026-01-13 to 2026-01-14; not copied |
| gemini-728-prompt | gemini:728 response → gemini:728 prompt | Gemini web (lineage), thread th_c26ead8e, 2026-01-13 to 2026-01-14; not copied |
| gemini-729-prompt | gemini:729 response → gemini:729 prompt | Gemini web (lineage), thread th_c26ead8e, 2026-01-13 to 2026-01-14; not copied |
| gemini-730-prompt | gemini:730 response → gemini:730 prompt | Gemini web (lineage), thread th_c26ead8e, 2026-01-13 to 2026-01-14; not copied |
| gemini-731-prompt | gemini:731 response → gemini:731 prompt | Gemini web (lineage), thread th_c26ead8e, 2026-01-13 to 2026-01-14; not copied |
| gemini-732-prompt | gemini:732 response → gemini:732 prompt | Gemini web (lineage), thread th_c26ead8e, 2026-01-13 to 2026-01-14; not copied |
| gemini-733-prompt | gemini:733 response → gemini:733 prompt | Gemini web (lineage), thread th_c26ead8e, 2026-01-13 to 2026-01-14; not copied |
| gemini-734-prompt | gemini:734 response → gemini:734 prompt | Gemini web (lineage), thread th_c26ead8e, 2026-01-13 to 2026-01-14; not copied |
| gemini-1204-prompt | gemini:1204 response → gemini:1204 prompt | Gemini web (lineage), thread th_c290798c, 2026-01-28; not copied |
| gemini-663-prompt | gemini:663 response → gemini:663 prompt | Gemini web (lineage), thread th_c2953c7a, 2026-01-12; not copied |
| gemini-1269-prompt | gemini:1269 response → gemini:1269 prompt | Gemini web (lineage), thread th_c2b40fe8, 2026-02-01; not copied |
| gemini-2259-prompt | gemini:2259 response → gemini:2259 prompt | Gemini web (lineage), thread th_c2c5fdbe, 2026-02-28; not copied |
| gemini-2260-prompt | gemini:2260 response → gemini:2260 prompt | Gemini web (lineage), thread th_c2c5fdbe, 2026-02-28; not copied |
| gemini-2261-prompt | gemini:2261 response → gemini:2261 prompt | Gemini web (lineage), thread th_c2c5fdbe, 2026-02-28; not copied |
| gemini-2262-prompt | gemini:2262 response → gemini:2262 prompt | Gemini web (lineage), thread th_c2c5fdbe, 2026-02-28; not copied |
| gemini-2263-prompt | gemini:2263 response → gemini:2263 prompt | Gemini web (lineage), thread th_c2c5fdbe, 2026-02-28; copied: 0 pasted, 1 lifted into archive notes |
| gemini-1105-prompt | gemini:1105 response → gemini:1105 prompt | Gemini web (lineage), thread th_c2dbd95c, 2026-01-26; copied: 0 pasted, 1 lifted into archive notes |
| gemini-1106-prompt | gemini:1106 response → gemini:1106 prompt | Gemini web (lineage), thread th_c2dbd95c, 2026-01-26; not copied |
| gemini-1107-prompt | gemini:1107 response → gemini:1107 prompt | Gemini web (lineage), thread th_c2dbd95c, 2026-01-26; copied: 2 pasted, 0 lifted into archive notes |
| gemini-153-prompt | gemini:153 response → gemini:153 prompt | Gemini web (lineage), thread th_c2f66ed4, 2025-12-04; not copied |
| gemini-330-prompt | gemini:330 response → gemini:330 prompt | Gemini web (lineage), thread th_c33dc15b, 2025-12-08; copied: 1 pasted, 0 lifted into archive notes |
| gemini-1249-prompt | gemini:1249 response → gemini:1249 prompt | Gemini web (lineage), thread th_c3977ce4, 2026-01-31; not copied |
| gemini-802-prompt | gemini:802 response → gemini:802 prompt | Gemini web (lineage), thread th_c3d1ffda, 2026-01-18; copied: 2 pasted, 0 lifted into archive notes |
| gemini-803-prompt | gemini:803 response → gemini:803 prompt | Gemini web (lineage), thread th_c3d1ffda, 2026-01-18; not copied |
| gemini-804-prompt | gemini:804 response → gemini:804 prompt | Gemini web (lineage), thread th_c3d1ffda, 2026-01-18; not copied |
| gemini-2853-prompt | gemini:2853 response → gemini:2853 prompt | Gemini web (lineage), thread th_c4148bc7, 2026-03-23; not copied |
| gemini-2854-prompt | gemini:2854 response → gemini:2854 prompt | Gemini web (lineage), thread th_c4148bc7, 2026-03-23; copied: 5 pasted, 0 lifted into archive notes |
| gemini-3008-prompt | gemini:3008 response → gemini:3008 prompt | Gemini web (lineage), thread th_c4191e3e, 2026-03-27; not copied |
| gemini-3009-prompt | gemini:3009 response → gemini:3009 prompt | Gemini web (lineage), thread th_c4191e3e, 2026-03-27; not copied |
| gemini-2841-prompt | gemini:2841 response → gemini:2841 prompt | Gemini web (lineage), thread th_c463b249, 2026-03-22; not copied |
| gemini-2842-prompt | gemini:2842 response → gemini:2842 prompt | Gemini web (lineage), thread th_c463b249, 2026-03-22; copied: 3 pasted, 0 lifted into archive notes |
| gemini-2122-prompt | gemini:2122 response → gemini:2122 prompt | Gemini web (lineage), thread th_c4a960e0, 2026-02-25; not copied |
| gemini-2123-prompt | gemini:2123 response → gemini:2123 prompt | Gemini web (lineage), thread th_c4a960e0, 2026-02-25; not copied |
| gemini-2124-prompt | gemini:2124 response → gemini:2124 prompt | Gemini web (lineage), thread th_c4a960e0, 2026-02-25; not copied |
| gemini-2125-prompt | gemini:2125 response → gemini:2125 prompt | Gemini web (lineage), thread th_c4a960e0, 2026-02-25; not copied |
| gemini-2126-prompt | gemini:2126 response → gemini:2126 prompt | Gemini web (lineage), thread th_c4a960e0, 2026-02-25; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2127-prompt | gemini:2127 response → gemini:2127 prompt | Gemini web (lineage), thread th_c4a960e0, 2026-02-25; copied: 1 pasted, 1 lifted into archive notes |
| gemini-2128-prompt | gemini:2128 response → gemini:2128 prompt | Gemini web (lineage), thread th_c4a960e0, 2026-02-25; copied: 3 pasted, 0 lifted into archive notes |
| gemini-2129-prompt | gemini:2129 response → gemini:2129 prompt | Gemini web (lineage), thread th_c4a960e0, 2026-02-25; copied: 3 pasted, 0 lifted into archive notes |
| gemini-2130-prompt | gemini:2130 response → gemini:2130 prompt | Gemini web (lineage), thread th_c4a960e0, 2026-02-25; copied: 5 pasted, 1 lifted into archive notes |
| gemini-1452-prompt | gemini:1452 response → gemini:1452 prompt | Gemini web (lineage), thread th_c5fcbbb2, 2026-02-07; copied: 2 pasted, 0 lifted into archive notes |
| gemini-246-prompt | gemini:246 response → gemini:246 prompt | Gemini web (lineage), thread th_c634d226, 2025-12-06; copied: 3 pasted, 1 lifted into archive notes |
| gemini-247-prompt | gemini:247 response → gemini:247 prompt | Gemini web (lineage), thread th_c634d226, 2025-12-06; copied: 2 pasted, 0 lifted into archive notes |
| gemini-248-prompt | gemini:248 response → gemini:248 prompt | Gemini web (lineage), thread th_c634d226, 2025-12-06; copied: 6 pasted, 0 lifted into archive notes |
| gemini-219-prompt | gemini:219 response → gemini:219 prompt | Gemini web (lineage), thread th_c63a96ba, 2025-12-05; not copied |
| gemini-220-prompt | gemini:220 response → gemini:220 prompt | Gemini web (lineage), thread th_c63a96ba, 2025-12-05; not copied |
| gemini-3119-prompt | gemini:3119 response → gemini:3119 prompt | Gemini web (lineage), thread th_c6a06885, 2026-04-08; not copied |
| gemini-3120-prompt | gemini:3120 response → gemini:3120 prompt | Gemini web (lineage), thread th_c6a06885, 2026-04-08; not copied |
| gemini-3121-prompt | gemini:3121 response → gemini:3121 prompt | Gemini web (lineage), thread th_c6a06885, 2026-04-08; not copied |
| gemini-3122-prompt | gemini:3122 response → gemini:3122 prompt | Gemini web (lineage), thread th_c6a06885, 2026-04-08; not copied |
| gemini-3123-prompt | gemini:3123 response → gemini:3123 prompt | Gemini web (lineage), thread th_c6a06885, 2026-04-08; not copied |
| gemini-3124-prompt | gemini:3124 response → gemini:3124 prompt | Gemini web (lineage), thread th_c6a06885, 2026-04-08; not copied |
| gemini-2381-prompt | gemini:2381 response → gemini:2381 prompt | Gemini web (lineage), thread th_c6ab82c7, 2026-03-04; copied: 3 pasted, 1 lifted into archive notes |
| gemini-2382-prompt | gemini:2382 response → gemini:2382 prompt | Gemini web (lineage), thread th_c6ab82c7, 2026-03-04; copied: 1 pasted, 1 lifted into archive notes |
| gemini-1069-prompt | gemini:1069 response → gemini:1069 prompt | Gemini web (lineage), thread th_c7071084, 2026-01-25; not copied |
| gemini-151-prompt | gemini:151 response → gemini:151 prompt | Gemini web (lineage), thread th_c7e8a15d, 2025-12-04; copied: 7 pasted, 0 lifted into archive notes |
| gemini-152-prompt | gemini:152 response → gemini:152 prompt | Gemini web (lineage), thread th_c7e8a15d, 2025-12-04; copied: 2 pasted, 0 lifted into archive notes |
| gemini-74-prompt | gemini:74 response → gemini:74 prompt | Gemini web (lineage), thread th_c84f71e8, 2025-12-01; not copied |
| gemini-75-prompt | gemini:75 response → gemini:75 prompt | Gemini web (lineage), thread th_c84f71e8, 2025-12-01; not copied |
| gemini-2681-prompt | gemini:2681 response → gemini:2681 prompt | Gemini web (lineage), thread th_c86356f5, 2026-03-13; not copied |
| gemini-2682-prompt | gemini:2682 response → gemini:2682 prompt | Gemini web (lineage), thread th_c86356f5, 2026-03-13; copied: 1 pasted, 0 lifted into archive notes |
| gemini-1187-prompt | gemini:1187 response → gemini:1187 prompt | Gemini web (lineage), thread th_c89794bb, 2026-01-27; copied: 8 pasted, 0 lifted into archive notes |
| gemini-1188-prompt | gemini:1188 response → gemini:1188 prompt | Gemini web (lineage), thread th_c89794bb, 2026-01-27; copied: 6 pasted, 0 lifted into archive notes |
| gemini-1189-prompt | gemini:1189 response → gemini:1189 prompt | Gemini web (lineage), thread th_c89794bb, 2026-01-27; not copied |
| gemini-1190-prompt | gemini:1190 response → gemini:1190 prompt | Gemini web (lineage), thread th_c89794bb, 2026-01-27; copied: 4 pasted, 0 lifted into archive notes |
| gemini-1191-prompt | gemini:1191 response → gemini:1191 prompt | Gemini web (lineage), thread th_c89794bb, 2026-01-27; not copied |
| gemini-1192-prompt | gemini:1192 response → gemini:1192 prompt | Gemini web (lineage), thread th_c89794bb, 2026-01-27; not copied |
| gemini-871-prompt | gemini:871 response → gemini:871 prompt | Gemini web (lineage), thread th_c8afcd4e, 2026-01-20; not copied |
| gemini-872-prompt | gemini:872 response → gemini:872 prompt | Gemini web (lineage), thread th_c8afcd4e, 2026-01-20; not copied |
| gemini-2484-prompt | gemini:2484 response → gemini:2484 prompt | Gemini web (lineage), thread th_c95f063d, 2026-03-06; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2485-prompt | gemini:2485 response → gemini:2485 prompt | Gemini web (lineage), thread th_c95f063d, 2026-03-06; not copied |
| gemini-2486-prompt | gemini:2486 response → gemini:2486 prompt | Gemini web (lineage), thread th_c95f063d, 2026-03-06; copied: 2 pasted, 0 lifted into archive notes |
| gemini-2487-prompt | gemini:2487 response → gemini:2487 prompt | Gemini web (lineage), thread th_c95f063d, 2026-03-06; copied: 2 pasted, 0 lifted into archive notes |
| gemini-2488-prompt | gemini:2488 response → gemini:2488 prompt | Gemini web (lineage), thread th_c95f063d, 2026-03-06; not copied |
| gemini-2489-prompt | gemini:2489 response → gemini:2489 prompt | Gemini web (lineage), thread th_c95f063d, 2026-03-06; not copied |
| gemini-2490-prompt | gemini:2490 response → gemini:2490 prompt | Gemini web (lineage), thread th_c95f063d, 2026-03-06; not copied |
| gemini-2491-prompt | gemini:2491 response → gemini:2491 prompt | Gemini web (lineage), thread th_c95f063d, 2026-03-06; copied: 1 pasted, 0 lifted into archive notes |
| gemini-222-prompt | gemini:222 response → gemini:222 prompt | Gemini web (lineage), thread th_c97e51e6, 2025-12-05; not copied |
| gemini-457-prompt | gemini:457 response → gemini:457 prompt | Gemini web (lineage), thread th_ca0e3e41, 2026-01-08; not copied |
| gemini-458-prompt | gemini:458 response → gemini:458 prompt | Gemini web (lineage), thread th_ca0e3e41, 2026-01-08; not copied |
| gemini-459-prompt | gemini:459 response → gemini:459 prompt | Gemini web (lineage), thread th_ca0e3e41, 2026-01-08; not copied |
| gemini-460-prompt | gemini:460 response → gemini:460 prompt | Gemini web (lineage), thread th_ca0e3e41, 2026-01-08; not copied |
| gemini-461-prompt | gemini:461 response → gemini:461 prompt | Gemini web (lineage), thread th_ca0e3e41, 2026-01-08; not copied |
| gemini-462-prompt | gemini:462 response → gemini:462 prompt | Gemini web (lineage), thread th_ca0e3e41, 2026-01-08; not copied |
| gemini-463-prompt | gemini:463 response → gemini:463 prompt | Gemini web (lineage), thread th_ca0e3e41, 2026-01-08; not copied |
| gemini-464-prompt | gemini:464 response → gemini:464 prompt | Gemini web (lineage), thread th_ca0e3e41, 2026-01-08; copied: 0 pasted, 1 lifted into archive notes |
| gemini-1885-prompt | gemini:1885 response → gemini:1885 prompt | Gemini web (lineage), thread th_ca2fd493, 2026-02-17; not copied |
| gemini-1886-prompt | gemini:1886 response → gemini:1886 prompt | Gemini web (lineage), thread th_ca2fd493, 2026-02-17; not copied |
| gemini-1887-prompt | gemini:1887 response → gemini:1887 prompt | Gemini web (lineage), thread th_ca2fd493, 2026-02-17; not copied |
| gemini-1888-prompt | gemini:1888 response → gemini:1888 prompt | Gemini web (lineage), thread th_ca2fd493, 2026-02-17; not copied |
| gemini-1889-prompt | gemini:1889 response → gemini:1889 prompt | Gemini web (lineage), thread th_ca2fd493, 2026-02-17; not copied |
| gemini-1890-prompt | gemini:1890 response → gemini:1890 prompt | Gemini web (lineage), thread th_ca2fd493, 2026-02-17; not copied |
| gemini-1891-prompt | gemini:1891 response → gemini:1891 prompt | Gemini web (lineage), thread th_ca2fd493, 2026-02-17; not copied |
| gemini-1892-prompt | gemini:1892 response → gemini:1892 prompt | Gemini web (lineage), thread th_ca2fd493, 2026-02-17; not copied |
| gemini-1893-prompt | gemini:1893 response → gemini:1893 prompt | Gemini web (lineage), thread th_ca2fd493, 2026-02-17; not copied |
| gemini-1894-prompt | gemini:1894 response → gemini:1894 prompt | Gemini web (lineage), thread th_ca2fd493, 2026-02-17; not copied |
| gemini-1895-prompt | gemini:1895 response → gemini:1895 prompt | Gemini web (lineage), thread th_ca2fd493, 2026-02-17; not copied |
| gemini-1896-prompt | gemini:1896 response → gemini:1896 prompt | Gemini web (lineage), thread th_ca2fd493, 2026-02-17; not copied |
| gemini-1897-prompt | gemini:1897 response → gemini:1897 prompt | Gemini web (lineage), thread th_ca2fd493, 2026-02-17; not copied |
| gemini-1898-prompt | gemini:1898 response → gemini:1898 prompt | Gemini web (lineage), thread th_ca2fd493, 2026-02-17; not copied |
| gemini-1899-prompt | gemini:1899 response → gemini:1899 prompt | Gemini web (lineage), thread th_ca2fd493, 2026-02-17; not copied |
| gemini-414-prompt | gemini:414 response → gemini:414 prompt | Gemini web (lineage), thread th_ca352847, 2025-12-28; not copied |
| gemini-415-prompt | gemini:415 response → gemini:415 prompt | Gemini web (lineage), thread th_ca352847, 2025-12-28; not copied |
| gemini-2607-prompt | gemini:2607 response → gemini:2607 prompt | Gemini web (lineage), thread th_cac6571c, 2026-03-09; not copied |
| gemini-2608-prompt | gemini:2608 response → gemini:2608 prompt | Gemini web (lineage), thread th_cac6571c, 2026-03-09; not copied |
| gemini-2609-prompt | gemini:2609 response → gemini:2609 prompt | Gemini web (lineage), thread th_cac6571c, 2026-03-09; not copied |
| gemini-2610-prompt | gemini:2610 response → gemini:2610 prompt | Gemini web (lineage), thread th_cac6571c, 2026-03-09; not copied |
| gemini-954-prompt | gemini:954 response → gemini:954 prompt | Gemini web (lineage), thread th_cadc6cc4, 2026-01-23; not copied |
| gemini-955-prompt | gemini:955 response → gemini:955 prompt | Gemini web (lineage), thread th_cadc6cc4, 2026-01-23; not copied |
| gemini-956-prompt | gemini:956 response → gemini:956 prompt | Gemini web (lineage), thread th_cadc6cc4, 2026-01-23; not copied |
| gemini-957-prompt | gemini:957 response → gemini:957 prompt | Gemini web (lineage), thread th_cadc6cc4, 2026-01-23; not copied |
| gemini-958-prompt | gemini:958 response → gemini:958 prompt | Gemini web (lineage), thread th_cadc6cc4, 2026-01-23; not copied |
| gemini-959-prompt | gemini:959 response → gemini:959 prompt | Gemini web (lineage), thread th_cadc6cc4, 2026-01-23; not copied |
| gemini-235-prompt | gemini:235 response → gemini:235 prompt | Gemini web (lineage), thread th_caddf2f0, 2025-12-05; not copied |
| gemini-578-prompt | gemini:578 response → gemini:578 prompt | Gemini web (lineage), thread th_caeb0d84, 2026-01-09; not copied |
| gemini-2469-prompt | gemini:2469 response → gemini:2469 prompt | Gemini web (lineage), thread th_cb0223f8, 2026-03-06; not copied |
| gemini-2676-prompt | gemini:2676 response → gemini:2676 prompt | Gemini web (lineage), thread th_cb44a258, 2026-03-12; not copied |
| gemini-2677-prompt | gemini:2677 response → gemini:2677 prompt | Gemini web (lineage), thread th_cb44a258, 2026-03-12; not copied |
| gemini-1855-prompt | gemini:1855 response → gemini:1855 prompt | Gemini web (lineage), thread th_cbbaad83, 2026-02-15; not copied |
| gemini-1856-prompt | gemini:1856 response → gemini:1856 prompt | Gemini web (lineage), thread th_cbbaad83, 2026-02-15; not copied |
| gemini-1857-prompt | gemini:1857 response → gemini:1857 prompt | Gemini web (lineage), thread th_cbbaad83, 2026-02-15; not copied |
| gemini-3157-prompt | gemini:3157 response → gemini:3157 prompt | Gemini web (lineage), thread th_cc6373f2, 2026-04-12; not copied |
| gemini-3085-prompt | gemini:3085 response → gemini:3085 prompt | Gemini web (lineage), thread th_ccbe4912, 2026-04-03; not copied |
| gemini-1474-prompt | gemini:1474 response → gemini:1474 prompt | Gemini web (lineage), thread th_cccebf31, 2026-02-08; not copied |
| gemini-1475-prompt | gemini:1475 response → gemini:1475 prompt | Gemini web (lineage), thread th_cccebf31, 2026-02-08; not copied |
| gemini-1476-prompt | gemini:1476 response → gemini:1476 prompt | Gemini web (lineage), thread th_cccebf31, 2026-02-08; not copied |
| gemini-1477-prompt | gemini:1477 response → gemini:1477 prompt | Gemini web (lineage), thread th_cccebf31, 2026-02-08; not copied |
| gemini-1478-prompt | gemini:1478 response → gemini:1478 prompt | Gemini web (lineage), thread th_cccebf31, 2026-02-08; not copied |
| gemini-1479-prompt | gemini:1479 response → gemini:1479 prompt | Gemini web (lineage), thread th_cccebf31, 2026-02-08; not copied |
| gemini-1480-prompt | gemini:1480 response → gemini:1480 prompt | Gemini web (lineage), thread th_cccebf31, 2026-02-08; not copied |
| gemini-1481-prompt | gemini:1481 response → gemini:1481 prompt | Gemini web (lineage), thread th_cccebf31, 2026-02-08; not copied |
| gemini-1482-prompt | gemini:1482 response → gemini:1482 prompt | Gemini web (lineage), thread th_cccebf31, 2026-02-08; not copied |
| gemini-1483-prompt | gemini:1483 response → gemini:1483 prompt | Gemini web (lineage), thread th_cccebf31, 2026-02-08; not copied |
| gemini-451-prompt | gemini:451 response → gemini:451 prompt | Gemini web (lineage), thread th_cd9d48f4, 2026-01-06; not copied |
| gemini-2921-prompt | gemini:2921 response → gemini:2921 prompt | Gemini web (lineage), thread th_cdae743b, 2026-03-24; not copied |
| gemini-1727-prompt | gemini:1727 response → gemini:1727 prompt | Gemini web (lineage), thread th_cdb4998b, 2026-02-11; not copied |
| gemini-1728-prompt | gemini:1728 response → gemini:1728 prompt | Gemini web (lineage), thread th_cdb4998b, 2026-02-11; not copied |
| gemini-1729-prompt | gemini:1729 response → gemini:1729 prompt | Gemini web (lineage), thread th_cdb4998b, 2026-02-11; not copied |
| gemini-431-prompt | gemini:431 response → gemini:431 prompt | Gemini web (lineage), thread th_cdd35003, 2025-12-29; not copied |
| gemini-432-prompt | gemini:432 response → gemini:432 prompt | Gemini web (lineage), thread th_cdd35003, 2025-12-29; not copied |
| gemini-2620-prompt | gemini:2620 response → gemini:2620 prompt | Gemini web (lineage), thread th_cdd4e01d, 2026-03-10; not copied |
| gemini-172-prompt | gemini:172 response → gemini:172 prompt | Gemini web (lineage), thread th_cde3fb46, 2025-12-04; copied: 2 pasted, 0 lifted into archive notes |
| gemini-2231-prompt | gemini:2231 response → gemini:2231 prompt | Gemini web (lineage), thread th_ce7c155c, 2026-02-27; not copied |
| gemini-2232-prompt | gemini:2232 response → gemini:2232 prompt | Gemini web (lineage), thread th_ce7c155c, 2026-02-27; not copied |
| gemini-786-prompt | gemini:786 response → gemini:786 prompt | Gemini web (lineage), thread th_cea458e8, 2026-01-17; not copied |
| gemini-2868-prompt | gemini:2868 response → gemini:2868 prompt | Gemini web (lineage), thread th_cebd6414, 2026-03-23; not copied |
| gemini-2869-prompt | gemini:2869 response → gemini:2869 prompt | Gemini web (lineage), thread th_cebd6414, 2026-03-23; not copied |
| gemini-2870-prompt | gemini:2870 response → gemini:2870 prompt | Gemini web (lineage), thread th_cebd6414, 2026-03-23; not copied |
| gemini-2104-prompt | gemini:2104 response → gemini:2104 prompt | Gemini web (lineage), thread th_cf667506, 2026-02-25; not copied |
| gemini-2105-prompt | gemini:2105 response → gemini:2105 prompt | Gemini web (lineage), thread th_cf667506, 2026-02-25; copied: 2 pasted, 0 lifted into archive notes |
| gemini-2106-prompt | gemini:2106 response → gemini:2106 prompt | Gemini web (lineage), thread th_cf667506, 2026-02-25; copied: 9 pasted, 1 lifted into archive notes |
| gemini-1342-prompt | gemini:1342 response → gemini:1342 prompt | Gemini web (lineage), thread th_cf904665, 2026-02-04; copied: 1 pasted, 0 lifted into archive notes |
| gemini-1343-prompt | gemini:1343 response → gemini:1343 prompt | Gemini web (lineage), thread th_cf904665, 2026-02-04; copied: 1 pasted, 1 lifted into archive notes |
| gemini-1344-prompt | gemini:1344 response → gemini:1344 prompt | Gemini web (lineage), thread th_cf904665, 2026-02-04; copied: 4 pasted, 0 lifted into archive notes |
| gemini-1763-prompt | gemini:1763 response → gemini:1763 prompt | Gemini web (lineage), thread th_cff10d78, 2026-02-12; not copied |
| gemini-1764-prompt | gemini:1764 response → gemini:1764 prompt | Gemini web (lineage), thread th_cff10d78, 2026-02-12; not copied |
| gemini-1765-prompt | gemini:1765 response → gemini:1765 prompt | Gemini web (lineage), thread th_cff10d78, 2026-02-12; not copied |
| gemini-1766-prompt | gemini:1766 response → gemini:1766 prompt | Gemini web (lineage), thread th_cff10d78, 2026-02-12; not copied |
| gemini-1767-prompt | gemini:1767 response → gemini:1767 prompt | Gemini web (lineage), thread th_cff10d78, 2026-02-12; not copied |
| gemini-1768-prompt | gemini:1768 response → gemini:1768 prompt | Gemini web (lineage), thread th_cff10d78, 2026-02-12; copied: 1 pasted, 0 lifted into archive notes |
| gemini-1769-prompt | gemini:1769 response → gemini:1769 prompt | Gemini web (lineage), thread th_cff10d78, 2026-02-12; not copied |
| gemini-1002-prompt | gemini:1002 response → gemini:1002 prompt | Gemini web (lineage), thread th_d0e4756d, 2026-01-24; not copied |
| gemini-199-prompt | gemini:199 response → gemini:199 prompt | Gemini web (lineage), thread th_d0ee5c13, 2025-12-05; copied: 1 pasted, 0 lifted into archive notes |
| gemini-1257-prompt | gemini:1257 response → gemini:1257 prompt | Gemini web (lineage), thread th_d0f5d402, 2026-01-31; not copied |
| gemini-1258-prompt | gemini:1258 response → gemini:1258 prompt | Gemini web (lineage), thread th_d0f5d402, 2026-01-31; not copied |
| gemini-1259-prompt | gemini:1259 response → gemini:1259 prompt | Gemini web (lineage), thread th_d0f5d402, 2026-01-31; not copied |
| gemini-2873-prompt | gemini:2873 response → gemini:2873 prompt | Gemini web (lineage), thread th_d0fd30e1, 2026-03-23; copied: 0 pasted, 1 lifted into archive notes |
| gemini-2874-prompt | gemini:2874 response → gemini:2874 prompt | Gemini web (lineage), thread th_d0fd30e1, 2026-03-23; copied: 2 pasted, 0 lifted into archive notes |
| gemini-2875-prompt | gemini:2875 response → gemini:2875 prompt | Gemini web (lineage), thread th_d0fd30e1, 2026-03-23; not copied |
| gemini-2876-prompt | gemini:2876 response → gemini:2876 prompt | Gemini web (lineage), thread th_d0fd30e1, 2026-03-23; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2509-prompt | gemini:2509 response → gemini:2509 prompt | Gemini web (lineage), thread th_d143c341, 2026-03-06; not copied |
| gemini-1780-prompt | gemini:1780 response → gemini:1780 prompt | Gemini web (lineage), thread th_d14a794c, 2026-02-13; not copied |
| gemini-2206-prompt | gemini:2206 response → gemini:2206 prompt | Gemini web (lineage), thread th_d15c1328, 2026-02-27; not copied |
| gemini-2207-prompt | gemini:2207 response → gemini:2207 prompt | Gemini web (lineage), thread th_d15c1328, 2026-02-27; not copied |
| gemini-2208-prompt | gemini:2208 response → gemini:2208 prompt | Gemini web (lineage), thread th_d15c1328, 2026-02-27; not copied |
| gemini-2209-prompt | gemini:2209 response → gemini:2209 prompt | Gemini web (lineage), thread th_d15c1328, 2026-02-27; not copied |
| gemini-2210-prompt | gemini:2210 response → gemini:2210 prompt | Gemini web (lineage), thread th_d15c1328, 2026-02-27; not copied |
| gemini-2211-prompt | gemini:2211 response → gemini:2211 prompt | Gemini web (lineage), thread th_d15c1328, 2026-02-27; not copied |
| gemini-2212-prompt | gemini:2212 response → gemini:2212 prompt | Gemini web (lineage), thread th_d15c1328, 2026-02-27; not copied |
| gemini-2213-prompt | gemini:2213 response → gemini:2213 prompt | Gemini web (lineage), thread th_d15c1328, 2026-02-27; not copied |
| gemini-2254-prompt | gemini:2254 response → gemini:2254 prompt | Gemini web (lineage), thread th_d17e391f, 2026-02-28; not copied |
| gemini-2400-prompt | gemini:2400 response → gemini:2400 prompt | Gemini web (lineage), thread th_d1eae79c, 2026-03-05; copied: 1 pasted, 0 lifted into archive notes |
| gemini-1110-prompt | gemini:1110 response → gemini:1110 prompt | Gemini web (lineage), thread th_d23223e3, 2026-01-26; not copied |
| gemini-373-prompt | gemini:373 response → gemini:373 prompt | Gemini web (lineage), thread th_d2732391, 2025-12-28; not copied |
| gemini-374-prompt | gemini:374 response → gemini:374 prompt | Gemini web (lineage), thread th_d2732391, 2025-12-28; not copied |
| gemini-375-prompt | gemini:375 response → gemini:375 prompt | Gemini web (lineage), thread th_d2732391, 2025-12-28; not copied |
| gemini-646-prompt | gemini:646 response → gemini:646 prompt | Gemini web (lineage), thread th_d2ff2485, 2026-01-11; not copied |
| gemini-2264-prompt | gemini:2264 response → gemini:2264 prompt | Gemini web (lineage), thread th_d32f3c07, 2026-03-01; not copied |
| gemini-2265-prompt | gemini:2265 response → gemini:2265 prompt | Gemini web (lineage), thread th_d32f3c07, 2026-03-01; not copied |
| gemini-2266-prompt | gemini:2266 response → gemini:2266 prompt | Gemini web (lineage), thread th_d32f3c07, 2026-03-01; not copied |
| gemini-2267-prompt | gemini:2267 response → gemini:2267 prompt | Gemini web (lineage), thread th_d32f3c07, 2026-03-01; not copied |
| gemini-2268-prompt | gemini:2268 response → gemini:2268 prompt | Gemini web (lineage), thread th_d32f3c07, 2026-03-01; not copied |
| gemini-2269-prompt | gemini:2269 response → gemini:2269 prompt | Gemini web (lineage), thread th_d32f3c07, 2026-03-01; not copied |
| gemini-2270-prompt | gemini:2270 response → gemini:2270 prompt | Gemini web (lineage), thread th_d32f3c07, 2026-03-01; not copied |
| gemini-2271-prompt | gemini:2271 response → gemini:2271 prompt | Gemini web (lineage), thread th_d32f3c07, 2026-03-01; not copied |
| gemini-2272-prompt | gemini:2272 response → gemini:2272 prompt | Gemini web (lineage), thread th_d32f3c07, 2026-03-01; not copied |
| gemini-1050-prompt | gemini:1050 response → gemini:1050 prompt | Gemini web (lineage), thread th_d3473caf, 2026-01-25; not copied |
| gemini-1052-prompt | gemini:1052 response → gemini:1052 prompt | Gemini web (lineage), thread th_d3473caf, 2026-01-25; not copied |
| gemini-1053-prompt | gemini:1053 response → gemini:1053 prompt | Gemini web (lineage), thread th_d3473caf, 2026-01-25; not copied |
| gemini-1788-prompt | gemini:1788 response → gemini:1788 prompt | Gemini web (lineage), thread th_d3504929, 2026-02-13; not copied |
| gemini-1789-prompt | gemini:1789 response → gemini:1789 prompt | Gemini web (lineage), thread th_d3504929, 2026-02-13; not copied |
| gemini-1790-prompt | gemini:1790 response → gemini:1790 prompt | Gemini web (lineage), thread th_d3504929, 2026-02-13; not copied |
| gemini-1791-prompt | gemini:1791 response → gemini:1791 prompt | Gemini web (lineage), thread th_d3504929, 2026-02-13; not copied |
| gemini-1792-prompt | gemini:1792 response → gemini:1792 prompt | Gemini web (lineage), thread th_d3504929, 2026-02-13; not copied |
| gemini-1793-prompt | gemini:1793 response → gemini:1793 prompt | Gemini web (lineage), thread th_d3504929, 2026-02-13; not copied |
| gemini-1794-prompt | gemini:1794 response → gemini:1794 prompt | Gemini web (lineage), thread th_d3504929, 2026-02-13; not copied |
| gemini-1795-prompt | gemini:1795 response → gemini:1795 prompt | Gemini web (lineage), thread th_d3504929, 2026-02-13; not copied |
| gemini-795-prompt | gemini:795 response → gemini:795 prompt | Gemini web (lineage), thread th_d37d044d, 2026-01-17; not copied |
| gemini-2424-prompt | gemini:2424 response → gemini:2424 prompt | Gemini web (lineage), thread th_d3f62301, 2026-03-05; not copied |
| gemini-2425-prompt | gemini:2425 response → gemini:2425 prompt | Gemini web (lineage), thread th_d3f62301, 2026-03-05; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2426-prompt | gemini:2426 response → gemini:2426 prompt | Gemini web (lineage), thread th_d3f62301, 2026-03-05; not copied |
| gemini-2427-prompt | gemini:2427 response → gemini:2427 prompt | Gemini web (lineage), thread th_d3f62301, 2026-03-05; not copied |
| gemini-2428-prompt | gemini:2428 response → gemini:2428 prompt | Gemini web (lineage), thread th_d3f62301, 2026-03-05; not copied |
| gemini-2429-prompt | gemini:2429 response → gemini:2429 prompt | Gemini web (lineage), thread th_d3f62301, 2026-03-05; not copied |
| gemini-2430-prompt | gemini:2430 response → gemini:2430 prompt | Gemini web (lineage), thread th_d3f62301, 2026-03-05; not copied |
| gemini-2431-prompt | gemini:2431 response → gemini:2431 prompt | Gemini web (lineage), thread th_d3f62301, 2026-03-05; copied: 1 pasted, 0 lifted into archive notes |
| gemini-1157-prompt | gemini:1157 response → gemini:1157 prompt | Gemini web (lineage), thread th_d4900811, 2026-01-27; not copied |
| gemini-1158-prompt | gemini:1158 response → gemini:1158 prompt | Gemini web (lineage), thread th_d4900811, 2026-01-27; not copied |
| gemini-1159-prompt | gemini:1159 response → gemini:1159 prompt | Gemini web (lineage), thread th_d4900811, 2026-01-27; not copied |
| gemini-2107-prompt | gemini:2107 response → gemini:2107 prompt | Gemini web (lineage), thread th_d51e5aea, 2026-02-25; not copied |
| gemini-2108-prompt | gemini:2108 response → gemini:2108 prompt | Gemini web (lineage), thread th_d51e5aea, 2026-02-25; not copied |
| gemini-3090-prompt | gemini:3090 response → gemini:3090 prompt | Gemini web (lineage), thread th_d65ccff8, 2026-04-07; not copied |
| gemini-3091-prompt | gemini:3091 response → gemini:3091 prompt | Gemini web (lineage), thread th_d65ccff8, 2026-04-07; not copied |
| gemini-3092-prompt | gemini:3092 response → gemini:3092 prompt | Gemini web (lineage), thread th_d65ccff8, 2026-04-07; not copied |
| gemini-2324-prompt | gemini:2324 response → gemini:2324 prompt | Gemini web (lineage), thread th_d6b1ef2c, 2026-03-03; not copied |
| gemini-2325-prompt | gemini:2325 response → gemini:2325 prompt | Gemini web (lineage), thread th_d6b1ef2c, 2026-03-03; not copied |
| gemini-2326-prompt | gemini:2326 response → gemini:2326 prompt | Gemini web (lineage), thread th_d6b1ef2c, 2026-03-03; not copied |
| gemini-2327-prompt | gemini:2327 response → gemini:2327 prompt | Gemini web (lineage), thread th_d6b1ef2c, 2026-03-03; not copied |
| gemini-2328-prompt | gemini:2328 response → gemini:2328 prompt | Gemini web (lineage), thread th_d6b1ef2c, 2026-03-03; not copied |
| gemini-2329-prompt | gemini:2329 response → gemini:2329 prompt | Gemini web (lineage), thread th_d6b1ef2c, 2026-03-03; not copied |
| gemini-2330-prompt | gemini:2330 response → gemini:2330 prompt | Gemini web (lineage), thread th_d6b1ef2c, 2026-03-03; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2331-prompt | gemini:2331 response → gemini:2331 prompt | Gemini web (lineage), thread th_d6b1ef2c, 2026-03-03; not copied |
| gemini-2332-prompt | gemini:2332 response → gemini:2332 prompt | Gemini web (lineage), thread th_d6b1ef2c, 2026-03-03; not copied |
| gemini-2333-prompt | gemini:2333 response → gemini:2333 prompt | Gemini web (lineage), thread th_d6b1ef2c, 2026-03-03; not copied |
| gemini-2334-prompt | gemini:2334 response → gemini:2334 prompt | Gemini web (lineage), thread th_d6b1ef2c, 2026-03-03; not copied |
| gemini-2335-prompt | gemini:2335 response → gemini:2335 prompt | Gemini web (lineage), thread th_d6b1ef2c, 2026-03-03; not copied |
| gemini-2336-prompt | gemini:2336 response → gemini:2336 prompt | Gemini web (lineage), thread th_d6b1ef2c, 2026-03-03; not copied |
| gemini-2337-prompt | gemini:2337 response → gemini:2337 prompt | Gemini web (lineage), thread th_d6b1ef2c, 2026-03-03; not copied |
| gemini-2338-prompt | gemini:2338 response → gemini:2338 prompt | Gemini web (lineage), thread th_d6b1ef2c, 2026-03-03; not copied |
| gemini-2339-prompt | gemini:2339 response → gemini:2339 prompt | Gemini web (lineage), thread th_d6b1ef2c, 2026-03-03; not copied |
| gemini-2340-prompt | gemini:2340 response → gemini:2340 prompt | Gemini web (lineage), thread th_d6b1ef2c, 2026-03-03; not copied |
| gemini-2341-prompt | gemini:2341 response → gemini:2341 prompt | Gemini web (lineage), thread th_d6b1ef2c, 2026-03-03; not copied |
| gemini-2342-prompt | gemini:2342 response → gemini:2342 prompt | Gemini web (lineage), thread th_d6b1ef2c, 2026-03-03; not copied |
| gemini-1841-prompt | gemini:1841 response → gemini:1841 prompt | Gemini web (lineage), thread th_d6eb705d, 2026-02-15; not copied |
| gemini-1842-prompt | gemini:1842 response → gemini:1842 prompt | Gemini web (lineage), thread th_d6eb705d, 2026-02-15; not copied |
| gemini-1843-prompt | gemini:1843 response → gemini:1843 prompt | Gemini web (lineage), thread th_d6eb705d, 2026-02-15; not copied |
| gemini-1844-prompt | gemini:1844 response → gemini:1844 prompt | Gemini web (lineage), thread th_d6eb705d, 2026-02-15; not copied |
| gemini-1845-prompt | gemini:1845 response → gemini:1845 prompt | Gemini web (lineage), thread th_d6eb705d, 2026-02-15; not copied |
| gemini-1846-prompt | gemini:1846 response → gemini:1846 prompt | Gemini web (lineage), thread th_d6eb705d, 2026-02-15; not copied |
| gemini-1847-prompt | gemini:1847 response → gemini:1847 prompt | Gemini web (lineage), thread th_d6eb705d, 2026-02-15; not copied |
| gemini-1848-prompt | gemini:1848 response → gemini:1848 prompt | Gemini web (lineage), thread th_d6eb705d, 2026-02-15; not copied |
| gemini-1849-prompt | gemini:1849 response → gemini:1849 prompt | Gemini web (lineage), thread th_d6eb705d, 2026-02-15; not copied |
| gemini-1850-prompt | gemini:1850 response → gemini:1850 prompt | Gemini web (lineage), thread th_d6eb705d, 2026-02-15; not copied |
| gemini-1851-prompt | gemini:1851 response → gemini:1851 prompt | Gemini web (lineage), thread th_d6eb705d, 2026-02-15; not copied |
| gemini-1852-prompt | gemini:1852 response → gemini:1852 prompt | Gemini web (lineage), thread th_d6eb705d, 2026-02-15; not copied |
| gemini-1853-prompt | gemini:1853 response → gemini:1853 prompt | Gemini web (lineage), thread th_d6eb705d, 2026-02-15; not copied |
| gemini-909-prompt | gemini:909 response → gemini:909 prompt | Gemini web (lineage), thread th_d717bb5c, 2026-01-22; not copied |
| gemini-1131-prompt | gemini:1131 response → gemini:1131 prompt | Gemini web (lineage), thread th_d723d21c, 2026-01-27; not copied |
| gemini-1722-prompt | gemini:1722 response → gemini:1722 prompt | Gemini web (lineage), thread th_d7408bfb, 2026-02-11; not copied |
| gemini-1723-prompt | gemini:1723 response → gemini:1723 prompt | Gemini web (lineage), thread th_d7408bfb, 2026-02-11; not copied |
| gemini-1724-prompt | gemini:1724 response → gemini:1724 prompt | Gemini web (lineage), thread th_d7408bfb, 2026-02-11; not copied |
| gemini-1725-prompt | gemini:1725 response → gemini:1725 prompt | Gemini web (lineage), thread th_d7408bfb, 2026-02-11; not copied |
| gemini-1726-prompt | gemini:1726 response → gemini:1726 prompt | Gemini web (lineage), thread th_d7408bfb, 2026-02-11; not copied |
| gemini-1454-prompt | gemini:1454 response → gemini:1454 prompt | Gemini web (lineage), thread th_d75160ac, 2026-02-07; copied: 2 pasted, 1 lifted into archive notes |
| gemini-1962-prompt | gemini:1962 response → gemini:1962 prompt | Gemini web (lineage), thread th_d7649a37, 2026-02-20; not copied |
| gemini-1963-prompt | gemini:1963 response → gemini:1963 prompt | Gemini web (lineage), thread th_d7649a37, 2026-02-20; not copied |
| gemini-1964-prompt | gemini:1964 response → gemini:1964 prompt | Gemini web (lineage), thread th_d7649a37, 2026-02-20; not copied |
| gemini-1965-prompt | gemini:1965 response → gemini:1965 prompt | Gemini web (lineage), thread th_d7649a37, 2026-02-20; copied: 2 pasted, 0 lifted into archive notes |
| gemini-1966-prompt | gemini:1966 response → gemini:1966 prompt | Gemini web (lineage), thread th_d7649a37, 2026-02-20; copied: 0 pasted, 1 lifted into archive notes |
| gemini-1967-prompt | gemini:1967 response → gemini:1967 prompt | Gemini web (lineage), thread th_d7649a37, 2026-02-20; copied: 2 pasted, 0 lifted into archive notes |
| gemini-507-prompt | gemini:507 response → gemini:507 prompt | Gemini web (lineage), thread th_d79397df, 2026-01-09; not copied |
| gemini-2733-prompt | gemini:2733 response → gemini:2733 prompt | Gemini web (lineage), thread th_d7d89078, 2026-03-17; not copied |
| gemini-274-prompt | gemini:274 response → gemini:274 prompt | Gemini web (lineage), thread th_d7ef6ae6, 2025-12-06; copied: 2 pasted, 1 lifted into archive notes |
| gemini-275-prompt | gemini:275 response → gemini:275 prompt | Gemini web (lineage), thread th_d7ef6ae6, 2025-12-06; not copied |
| gemini-276-prompt | gemini:276 response → gemini:276 prompt | Gemini web (lineage), thread th_d7ef6ae6, 2025-12-06; copied: 1 pasted, 0 lifted into archive notes |
| gemini-277-prompt | gemini:277 response → gemini:277 prompt | Gemini web (lineage), thread th_d7ef6ae6, 2025-12-06; copied: 2 pasted, 0 lifted into archive notes |
| gemini-278-prompt | gemini:278 response → gemini:278 prompt | Gemini web (lineage), thread th_d7ef6ae6, 2025-12-06; copied: 1 pasted, 0 lifted into archive notes |
| gemini-279-prompt | gemini:279 response → gemini:279 prompt | Gemini web (lineage), thread th_d7ef6ae6, 2025-12-06; copied: 3 pasted, 0 lifted into archive notes |
| gemini-280-prompt | gemini:280 response → gemini:280 prompt | Gemini web (lineage), thread th_d7ef6ae6, 2025-12-06; copied: 6 pasted, 0 lifted into archive notes |
| gemini-1825-prompt | gemini:1825 response → gemini:1825 prompt | Gemini web (lineage), thread th_d81667af, 2026-02-13; not copied |
| gemini-1826-prompt | gemini:1826 response → gemini:1826 prompt | Gemini web (lineage), thread th_d81667af, 2026-02-13; not copied |
| gemini-1699-prompt | gemini:1699 response → gemini:1699 prompt | Gemini web (lineage), thread th_d8602feb, 2026-02-11; not copied |
| gemini-1700-prompt | gemini:1700 response → gemini:1700 prompt | Gemini web (lineage), thread th_d8602feb, 2026-02-11; not copied |
| gemini-1701-prompt | gemini:1701 response → gemini:1701 prompt | Gemini web (lineage), thread th_d8602feb, 2026-02-11; not copied |
| gemini-1702-prompt | gemini:1702 response → gemini:1702 prompt | Gemini web (lineage), thread th_d8602feb, 2026-02-11; not copied |
| gemini-1703-prompt | gemini:1703 response → gemini:1703 prompt | Gemini web (lineage), thread th_d8602feb, 2026-02-11; not copied |
| gemini-1704-prompt | gemini:1704 response → gemini:1704 prompt | Gemini web (lineage), thread th_d8602feb, 2026-02-11; not copied |
| gemini-1705-prompt | gemini:1705 response → gemini:1705 prompt | Gemini web (lineage), thread th_d8602feb, 2026-02-11; not copied |
| gemini-1706-prompt | gemini:1706 response → gemini:1706 prompt | Gemini web (lineage), thread th_d8602feb, 2026-02-11; not copied |
| gemini-1707-prompt | gemini:1707 response → gemini:1707 prompt | Gemini web (lineage), thread th_d8602feb, 2026-02-11; not copied |
| gemini-1708-prompt | gemini:1708 response → gemini:1708 prompt | Gemini web (lineage), thread th_d8602feb, 2026-02-11; not copied |
| gemini-1709-prompt | gemini:1709 response → gemini:1709 prompt | Gemini web (lineage), thread th_d8602feb, 2026-02-11; not copied |
| gemini-1710-prompt | gemini:1710 response → gemini:1710 prompt | Gemini web (lineage), thread th_d8602feb, 2026-02-11; not copied |
| gemini-620-prompt | gemini:620 response → gemini:620 prompt | Gemini web (lineage), thread th_d8827ba7, 2026-01-10; not copied |
| gemini-649-prompt | gemini:649 response → gemini:649 prompt | Gemini web (lineage), thread th_d8bd54fc, 2026-01-11; not copied |
| gemini-1907-prompt | gemini:1907 response → gemini:1907 prompt | Gemini web (lineage), thread th_d8d1df2f, 2026-02-19; not copied |
| gemini-1058-prompt | gemini:1058 response → gemini:1058 prompt | Gemini web (lineage), thread th_d9378cba, 2026-01-25; not copied |
| gemini-1059-prompt | gemini:1059 response → gemini:1059 prompt | Gemini web (lineage), thread th_d9378cba, 2026-01-25; not copied |
| gemini-1060-prompt | gemini:1060 response → gemini:1060 prompt | Gemini web (lineage), thread th_d9378cba, 2026-01-25; not copied |
| gemini-1061-prompt | gemini:1061 response → gemini:1061 prompt | Gemini web (lineage), thread th_d9378cba, 2026-01-25; copied: 1 pasted, 1 lifted into archive notes |
| gemini-1749-prompt | gemini:1749 response → gemini:1749 prompt | Gemini web (lineage), thread th_d968931a, 2026-02-12; not copied |
| gemini-1750-prompt | gemini:1750 response → gemini:1750 prompt | Gemini web (lineage), thread th_d968931a, 2026-02-12; not copied |
| gemini-1751-prompt | gemini:1751 response → gemini:1751 prompt | Gemini web (lineage), thread th_d968931a, 2026-02-12; not copied |
| gemini-1752-prompt | gemini:1752 response → gemini:1752 prompt | Gemini web (lineage), thread th_d968931a, 2026-02-12; not copied |
| gemini-1753-prompt | gemini:1753 response → gemini:1753 prompt | Gemini web (lineage), thread th_d968931a, 2026-02-12; not copied |
| gemini-1754-prompt | gemini:1754 response → gemini:1754 prompt | Gemini web (lineage), thread th_d968931a, 2026-02-12; copied: 1 pasted, 0 lifted into archive notes |
| gemini-1755-prompt | gemini:1755 response → gemini:1755 prompt | Gemini web (lineage), thread th_d968931a, 2026-02-12; copied: 4 pasted, 0 lifted into archive notes |
| gemini-1756-prompt | gemini:1756 response → gemini:1756 prompt | Gemini web (lineage), thread th_d968931a, 2026-02-12; copied: 2 pasted, 0 lifted into archive notes |
| gemini-1757-prompt | gemini:1757 response → gemini:1757 prompt | Gemini web (lineage), thread th_d968931a, 2026-02-12; copied: 5 pasted, 0 lifted into archive notes |
| gemini-429-prompt | gemini:429 response → gemini:429 prompt | Gemini web (lineage), thread th_d97273d3, 2025-12-29; not copied |
| gemini-430-prompt | gemini:430 response → gemini:430 prompt | Gemini web (lineage), thread th_d97273d3, 2025-12-29; not copied |
| gemini-2628-prompt | gemini:2628 response → gemini:2628 prompt | Gemini web (lineage), thread th_d9d28781, 2026-03-10; copied: 1 pasted, 0 lifted into archive notes |
| gemini-697-prompt | gemini:697 response → gemini:697 prompt | Gemini web (lineage), thread th_d9d2ada1, 2026-01-13; not copied |
| gemini-698-prompt | gemini:698 response → gemini:698 prompt | Gemini web (lineage), thread th_d9d2ada1, 2026-01-13; not copied |
| gemini-699-prompt | gemini:699 response → gemini:699 prompt | Gemini web (lineage), thread th_d9d2ada1, 2026-01-13; not copied |
| gemini-700-prompt | gemini:700 response → gemini:700 prompt | Gemini web (lineage), thread th_d9d2ada1, 2026-01-13; not copied |
| gemini-701-prompt | gemini:701 response → gemini:701 prompt | Gemini web (lineage), thread th_d9d2ada1, 2026-01-13; not copied |
| gemini-702-prompt | gemini:702 response → gemini:702 prompt | Gemini web (lineage), thread th_d9d2ada1, 2026-01-13; not copied |
| gemini-703-prompt | gemini:703 response → gemini:703 prompt | Gemini web (lineage), thread th_d9d2ada1, 2026-01-13; not copied |
| gemini-704-prompt | gemini:704 response → gemini:704 prompt | Gemini web (lineage), thread th_d9d2ada1, 2026-01-13; not copied |
| gemini-705-prompt | gemini:705 response → gemini:705 prompt | Gemini web (lineage), thread th_d9d2ada1, 2026-01-13; not copied |
| gemini-706-prompt | gemini:706 response → gemini:706 prompt | Gemini web (lineage), thread th_d9d2ada1, 2026-01-13; not copied |
| gemini-707-prompt | gemini:707 response → gemini:707 prompt | Gemini web (lineage), thread th_d9d2ada1, 2026-01-13; not copied |
| gemini-708-prompt | gemini:708 response → gemini:708 prompt | Gemini web (lineage), thread th_d9d2ada1, 2026-01-13; not copied |
| gemini-709-prompt | gemini:709 response → gemini:709 prompt | Gemini web (lineage), thread th_d9d2ada1, 2026-01-13; not copied |
| gemini-710-prompt | gemini:710 response → gemini:710 prompt | Gemini web (lineage), thread th_d9d2ada1, 2026-01-13; not copied |
| gemini-711-prompt | gemini:711 response → gemini:711 prompt | Gemini web (lineage), thread th_d9d2ada1, 2026-01-13; not copied |
| gemini-712-prompt | gemini:712 response → gemini:712 prompt | Gemini web (lineage), thread th_d9d2ada1, 2026-01-13; not copied |
| gemini-713-prompt | gemini:713 response → gemini:713 prompt | Gemini web (lineage), thread th_d9d2ada1, 2026-01-13; not copied |
| gemini-714-prompt | gemini:714 response → gemini:714 prompt | Gemini web (lineage), thread th_d9d2ada1, 2026-01-13; not copied |
| gemini-715-prompt | gemini:715 response → gemini:715 prompt | Gemini web (lineage), thread th_d9d2ada1, 2026-01-13; not copied |
| gemini-716-prompt | gemini:716 response → gemini:716 prompt | Gemini web (lineage), thread th_d9d2ada1, 2026-01-13; not copied |
| gemini-717-prompt | gemini:717 response → gemini:717 prompt | Gemini web (lineage), thread th_d9d2ada1, 2026-01-13; not copied |
| gemini-718-prompt | gemini:718 response → gemini:718 prompt | Gemini web (lineage), thread th_d9d2ada1, 2026-01-13; not copied |
| gemini-719-prompt | gemini:719 response → gemini:719 prompt | Gemini web (lineage), thread th_d9d2ada1, 2026-01-13; not copied |
| gemini-720-prompt | gemini:720 response → gemini:720 prompt | Gemini web (lineage), thread th_d9d2ada1, 2026-01-13; not copied |
| gemini-721-prompt | gemini:721 response → gemini:721 prompt | Gemini web (lineage), thread th_d9d2ada1, 2026-01-13; not copied |
| gemini-722-prompt | gemini:722 response → gemini:722 prompt | Gemini web (lineage), thread th_d9d2ada1, 2026-01-13; not copied |
| gemini-723-prompt | gemini:723 response → gemini:723 prompt | Gemini web (lineage), thread th_d9d2ada1, 2026-01-13; copied: 0 pasted, 1 lifted into archive notes |
| gemini-724-prompt | gemini:724 response → gemini:724 prompt | Gemini web (lineage), thread th_d9d2ada1, 2026-01-13; not copied |
| gemini-725-prompt | gemini:725 response → gemini:725 prompt | Gemini web (lineage), thread th_d9d2ada1, 2026-01-13; not copied |
| gemini-70-prompt | gemini:70 response → gemini:70 prompt | Gemini web (lineage), thread th_da0cc8d4, 2025-12-01; not copied |
| gemini-71-prompt | gemini:71 response → gemini:71 prompt | Gemini web (lineage), thread th_da0cc8d4, 2025-12-01; not copied |
| gemini-72-prompt | gemini:72 response → gemini:72 prompt | Gemini web (lineage), thread th_da0cc8d4, 2025-12-01; not copied |
| gemini-73-prompt | gemini:73 response → gemini:73 prompt | Gemini web (lineage), thread th_da0cc8d4, 2025-12-01; not copied |
| gemini-1672-prompt | gemini:1672 response → gemini:1672 prompt | Gemini web (lineage), thread th_da4297c4, 2026-02-10; not copied |
| gemini-1673-prompt | gemini:1673 response → gemini:1673 prompt | Gemini web (lineage), thread th_da4297c4, 2026-02-10; not copied |
| gemini-1674-prompt | gemini:1674 response → gemini:1674 prompt | Gemini web (lineage), thread th_da4297c4, 2026-02-10; not copied |
| gemini-1675-prompt | gemini:1675 response → gemini:1675 prompt | Gemini web (lineage), thread th_da4297c4, 2026-02-10; not copied |
| gemini-1676-prompt | gemini:1676 response → gemini:1676 prompt | Gemini web (lineage), thread th_da4297c4, 2026-02-10; not copied |
| gemini-1677-prompt | gemini:1677 response → gemini:1677 prompt | Gemini web (lineage), thread th_da4297c4, 2026-02-10; not copied |
| gemini-411-prompt | gemini:411 response → gemini:411 prompt | Gemini web (lineage), thread th_da7596e7, 2025-12-28; not copied |
| gemini-791-prompt | gemini:791 response → gemini:791 prompt | Gemini web (lineage), thread th_da89ef91, 2026-01-17; not copied |
| gemini-792-prompt | gemini:792 response → gemini:792 prompt | Gemini web (lineage), thread th_da89ef91, 2026-01-17; not copied |
| gemini-793-prompt | gemini:793 response → gemini:793 prompt | Gemini web (lineage), thread th_da89ef91, 2026-01-17; not copied |
| gemini-794-prompt | gemini:794 response → gemini:794 prompt | Gemini web (lineage), thread th_da89ef91, 2026-01-17; not copied |
| gemini-782-prompt | gemini:782 response → gemini:782 prompt | Gemini web (lineage), thread th_db2560f1, 2026-01-17; not copied |
| gemini-783-prompt | gemini:783 response → gemini:783 prompt | Gemini web (lineage), thread th_db2560f1, 2026-01-17; copied: 1 pasted, 0 lifted into archive notes |
| gemini-784-prompt | gemini:784 response → gemini:784 prompt | Gemini web (lineage), thread th_db2560f1, 2026-01-17; not copied |
| gemini-434-prompt | gemini:434 response → gemini:434 prompt | Gemini web (lineage), thread th_db6e46f7, 2025-12-29; not copied |
| gemini-435-prompt | gemini:435 response → gemini:435 prompt | Gemini web (lineage), thread th_db6e46f7, 2025-12-29; not copied |
| gemini-1388-prompt | gemini:1388 response → gemini:1388 prompt | Gemini web (lineage), thread th_db77b020, 2026-02-06; not copied |
| gemini-1389-prompt | gemini:1389 response → gemini:1389 prompt | Gemini web (lineage), thread th_db77b020, 2026-02-06; not copied |
| gemini-1390-prompt | gemini:1390 response → gemini:1390 prompt | Gemini web (lineage), thread th_db77b020, 2026-02-06; not copied |
| gemini-847-prompt | gemini:847 response → gemini:847 prompt | Gemini web (lineage), thread th_db868134, 2026-01-19; not copied |
| gemini-848-prompt | gemini:848 response → gemini:848 prompt | Gemini web (lineage), thread th_db868134, 2026-01-19; not copied |
| gemini-403-prompt | gemini:403 response → gemini:403 prompt | Gemini web (lineage), thread th_dc12b3e7, 2025-12-28; not copied |
| gemini-398-prompt | gemini:398 response → gemini:398 prompt | Gemini web (lineage), thread th_dc188259, 2025-12-28; not copied |
| gemini-485-prompt | gemini:485 response → gemini:485 prompt | Gemini web (lineage), thread th_dc284bf7, 2026-01-08; not copied |
| gemini-486-prompt | gemini:486 response → gemini:486 prompt | Gemini web (lineage), thread th_dc284bf7, 2026-01-08; not copied |
| gemini-487-prompt | gemini:487 response → gemini:487 prompt | Gemini web (lineage), thread th_dc284bf7, 2026-01-08; copied: 0 pasted, 1 lifted into archive notes |
| gemini-488-prompt | gemini:488 response → gemini:488 prompt | Gemini web (lineage), thread th_dc284bf7, 2026-01-08; not copied |
| gemini-489-prompt | gemini:489 response → gemini:489 prompt | Gemini web (lineage), thread th_dc284bf7, 2026-01-08; not copied |
| gemini-490-prompt | gemini:490 response → gemini:490 prompt | Gemini web (lineage), thread th_dc284bf7, 2026-01-08; not copied |
| gemini-491-prompt | gemini:491 response → gemini:491 prompt | Gemini web (lineage), thread th_dc284bf7, 2026-01-08; not copied |
| gemini-492-prompt | gemini:492 response → gemini:492 prompt | Gemini web (lineage), thread th_dc284bf7, 2026-01-08; not copied |
| gemini-493-prompt | gemini:493 response → gemini:493 prompt | Gemini web (lineage), thread th_dc284bf7, 2026-01-08; not copied |
| gemini-494-prompt | gemini:494 response → gemini:494 prompt | Gemini web (lineage), thread th_dc284bf7, 2026-01-08; not copied |
| gemini-495-prompt | gemini:495 response → gemini:495 prompt | Gemini web (lineage), thread th_dc284bf7, 2026-01-08; not copied |
| gemini-496-prompt | gemini:496 response → gemini:496 prompt | Gemini web (lineage), thread th_dc284bf7, 2026-01-08; not copied |
| gemini-497-prompt | gemini:497 response → gemini:497 prompt | Gemini web (lineage), thread th_dc284bf7, 2026-01-08; not copied |
| gemini-498-prompt | gemini:498 response → gemini:498 prompt | Gemini web (lineage), thread th_dc284bf7, 2026-01-08; not copied |
| gemini-499-prompt | gemini:499 response → gemini:499 prompt | Gemini web (lineage), thread th_dc284bf7, 2026-01-08; not copied |
| gemini-500-prompt | gemini:500 response → gemini:500 prompt | Gemini web (lineage), thread th_dc284bf7, 2026-01-08; not copied |
| gemini-659-prompt | gemini:659 response → gemini:659 prompt | Gemini web (lineage), thread th_dc372e66, 2026-01-12; not copied |
| gemini-660-prompt | gemini:660 response → gemini:660 prompt | Gemini web (lineage), thread th_dc372e66, 2026-01-12; not copied |
| gemini-661-prompt | gemini:661 response → gemini:661 prompt | Gemini web (lineage), thread th_dc372e66, 2026-01-12; not copied |
| gemini-662-prompt | gemini:662 response → gemini:662 prompt | Gemini web (lineage), thread th_dc372e66, 2026-01-12; not copied |
| gemini-2179-prompt | gemini:2179 response → gemini:2179 prompt | Gemini web (lineage), thread th_dc98de8f, 2026-02-26; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2180-prompt | gemini:2180 response → gemini:2180 prompt | Gemini web (lineage), thread th_dc98de8f, 2026-02-26; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2181-prompt | gemini:2181 response → gemini:2181 prompt | Gemini web (lineage), thread th_dc98de8f, 2026-02-26; not copied |
| gemini-2182-prompt | gemini:2182 response → gemini:2182 prompt | Gemini web (lineage), thread th_dc98de8f, 2026-02-26; not copied |
| gemini-2183-prompt | gemini:2183 response → gemini:2183 prompt | Gemini web (lineage), thread th_dc98de8f, 2026-02-26; copied: 1 pasted, 1 lifted into archive notes |
| gemini-2184-prompt | gemini:2184 response → gemini:2184 prompt | Gemini web (lineage), thread th_dc98de8f, 2026-02-26; copied: 3 pasted, 0 lifted into archive notes |
| gemini-2185-prompt | gemini:2185 response → gemini:2185 prompt | Gemini web (lineage), thread th_dc98de8f, 2026-02-26; copied: 4 pasted, 0 lifted into archive notes |
| gemini-2186-prompt | gemini:2186 response → gemini:2186 prompt | Gemini web (lineage), thread th_dc98de8f, 2026-02-26; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2187-prompt | gemini:2187 response → gemini:2187 prompt | Gemini web (lineage), thread th_dc98de8f, 2026-02-26; copied: 2 pasted, 0 lifted into archive notes |
| gemini-2188-prompt | gemini:2188 response → gemini:2188 prompt | Gemini web (lineage), thread th_dc98de8f, 2026-02-26; not copied |
| gemini-2189-prompt | gemini:2189 response → gemini:2189 prompt | Gemini web (lineage), thread th_dc98de8f, 2026-02-26; not copied |
| gemini-2190-prompt | gemini:2190 response → gemini:2190 prompt | Gemini web (lineage), thread th_dc98de8f, 2026-02-26; not copied |
| gemini-921-prompt | gemini:921 response → gemini:921 prompt | Gemini web (lineage), thread th_dca4b1c4, 2026-01-22; not copied |
| gemini-922-prompt | gemini:922 response → gemini:922 prompt | Gemini web (lineage), thread th_dca4b1c4, 2026-01-22; not copied |
| gemini-923-prompt | gemini:923 response → gemini:923 prompt | Gemini web (lineage), thread th_dca4b1c4, 2026-01-22; not copied |
| gemini-924-prompt | gemini:924 response → gemini:924 prompt | Gemini web (lineage), thread th_dca4b1c4, 2026-01-22; not copied |
| gemini-925-prompt | gemini:925 response → gemini:925 prompt | Gemini web (lineage), thread th_dca4b1c4, 2026-01-22; not copied |
| gemini-926-prompt | gemini:926 response → gemini:926 prompt | Gemini web (lineage), thread th_dca4b1c4, 2026-01-22; not copied |
| gemini-927-prompt | gemini:927 response → gemini:927 prompt | Gemini web (lineage), thread th_dca4b1c4, 2026-01-22; not copied |
| gemini-928-prompt | gemini:928 response → gemini:928 prompt | Gemini web (lineage), thread th_dca4b1c4, 2026-01-22; not copied |
| gemini-929-prompt | gemini:929 response → gemini:929 prompt | Gemini web (lineage), thread th_dca4b1c4, 2026-01-22; not copied |
| gemini-930-prompt | gemini:930 response → gemini:930 prompt | Gemini web (lineage), thread th_dca4b1c4, 2026-01-22; not copied |
| gemini-931-prompt | gemini:931 response → gemini:931 prompt | Gemini web (lineage), thread th_dca4b1c4, 2026-01-22; not copied |
| gemini-932-prompt | gemini:932 response → gemini:932 prompt | Gemini web (lineage), thread th_dca4b1c4, 2026-01-22; copied: 1 pasted, 0 lifted into archive notes |
| gemini-933-prompt | gemini:933 response → gemini:933 prompt | Gemini web (lineage), thread th_dca4b1c4, 2026-01-22; not copied |
| gemini-934-prompt | gemini:934 response → gemini:934 prompt | Gemini web (lineage), thread th_dca4b1c4, 2026-01-22; copied: 1 pasted, 0 lifted into archive notes |
| gemini-759-prompt | gemini:759 response → gemini:759 prompt | Gemini web (lineage), thread th_dca78915, 2026-01-17; not copied |
| gemini-1758-prompt | gemini:1758 response → gemini:1758 prompt | Gemini web (lineage), thread th_dd6d670e, 2026-02-12; not copied |
| gemini-1759-prompt | gemini:1759 response → gemini:1759 prompt | Gemini web (lineage), thread th_dd6d670e, 2026-02-12; not copied |
| gemini-1760-prompt | gemini:1760 response → gemini:1760 prompt | Gemini web (lineage), thread th_dd6d670e, 2026-02-12; not copied |
| gemini-1761-prompt | gemini:1761 response → gemini:1761 prompt | Gemini web (lineage), thread th_dd6d670e, 2026-02-12; not copied |
| gemini-1773-prompt | gemini:1773 response → gemini:1773 prompt | Gemini web (lineage), thread th_ddb1e6b9, 2026-02-12 to 2026-02-13; not copied |
| gemini-1774-prompt | gemini:1774 response → gemini:1774 prompt | Gemini web (lineage), thread th_ddb1e6b9, 2026-02-12 to 2026-02-13; not copied |
| gemini-1775-prompt | gemini:1775 response → gemini:1775 prompt | Gemini web (lineage), thread th_ddb1e6b9, 2026-02-12 to 2026-02-13; not copied |
| gemini-1776-prompt | gemini:1776 response → gemini:1776 prompt | Gemini web (lineage), thread th_ddb1e6b9, 2026-02-12 to 2026-02-13; not copied |
| gemini-1777-prompt | gemini:1777 response → gemini:1777 prompt | Gemini web (lineage), thread th_ddb1e6b9, 2026-02-12 to 2026-02-13; not copied |
| gemini-1778-prompt | gemini:1778 response → gemini:1778 prompt | Gemini web (lineage), thread th_ddb1e6b9, 2026-02-12 to 2026-02-13; not copied |
| gemini-1779-prompt | gemini:1779 response → gemini:1779 prompt | Gemini web (lineage), thread th_ddb1e6b9, 2026-02-12 to 2026-02-13; not copied |
| gemini-1364-prompt | gemini:1364 response → gemini:1364 prompt | Gemini web (lineage), thread th_dde1d646, 2026-02-05; copied: 1 pasted, 0 lifted into archive notes |
| gemini-1365-prompt | gemini:1365 response → gemini:1365 prompt | Gemini web (lineage), thread th_dde1d646, 2026-02-05; not copied |
| gemini-1366-prompt | gemini:1366 response → gemini:1366 prompt | Gemini web (lineage), thread th_dde1d646, 2026-02-05; not copied |
| gemini-1367-prompt | gemini:1367 response → gemini:1367 prompt | Gemini web (lineage), thread th_dde1d646, 2026-02-05; copied: 4 pasted, 0 lifted into archive notes |
| gemini-1368-prompt | gemini:1368 response → gemini:1368 prompt | Gemini web (lineage), thread th_dde1d646, 2026-02-05; not copied |
| gemini-1369-prompt | gemini:1369 response → gemini:1369 prompt | Gemini web (lineage), thread th_dde1d646, 2026-02-05; not copied |
| gemini-1370-prompt | gemini:1370 response → gemini:1370 prompt | Gemini web (lineage), thread th_dde1d646, 2026-02-05; not copied |
| gemini-1371-prompt | gemini:1371 response → gemini:1371 prompt | Gemini web (lineage), thread th_dde1d646, 2026-02-05; copied: 1 pasted, 0 lifted into archive notes |
| gemini-1372-prompt | gemini:1372 response → gemini:1372 prompt | Gemini web (lineage), thread th_dde1d646, 2026-02-05; not copied |
| gemini-1373-prompt | gemini:1373 response → gemini:1373 prompt | Gemini web (lineage), thread th_dde1d646, 2026-02-05; not copied |
| gemini-1374-prompt | gemini:1374 response → gemini:1374 prompt | Gemini web (lineage), thread th_dde1d646, 2026-02-05; not copied |
| gemini-1375-prompt | gemini:1375 response → gemini:1375 prompt | Gemini web (lineage), thread th_dde1d646, 2026-02-05; not copied |
| gemini-1376-prompt | gemini:1376 response → gemini:1376 prompt | Gemini web (lineage), thread th_dde1d646, 2026-02-05; not copied |
| gemini-1377-prompt | gemini:1377 response → gemini:1377 prompt | Gemini web (lineage), thread th_dde1d646, 2026-02-05; not copied |
| gemini-1378-prompt | gemini:1378 response → gemini:1378 prompt | Gemini web (lineage), thread th_dde1d646, 2026-02-05; copied: 2 pasted, 0 lifted into archive notes |
| gemini-1416-prompt | gemini:1416 response → gemini:1416 prompt | Gemini web (lineage), thread th_de868d9f, 2026-02-06 to 2026-02-07; copied: 4 pasted, 0 lifted into archive notes |
| gemini-1417-prompt | gemini:1417 response → gemini:1417 prompt | Gemini web (lineage), thread th_de868d9f, 2026-02-06 to 2026-02-07; not copied |
| gemini-1418-prompt | gemini:1418 response → gemini:1418 prompt | Gemini web (lineage), thread th_de868d9f, 2026-02-06 to 2026-02-07; not copied |
| gemini-1419-prompt | gemini:1419 response → gemini:1419 prompt | Gemini web (lineage), thread th_de868d9f, 2026-02-06 to 2026-02-07; not copied |
| gemini-1420-prompt | gemini:1420 response → gemini:1420 prompt | Gemini web (lineage), thread th_de868d9f, 2026-02-06 to 2026-02-07; not copied |
| gemini-1421-prompt | gemini:1421 response → gemini:1421 prompt | Gemini web (lineage), thread th_de868d9f, 2026-02-06 to 2026-02-07; not copied |
| gemini-1422-prompt | gemini:1422 response → gemini:1422 prompt | Gemini web (lineage), thread th_de868d9f, 2026-02-06 to 2026-02-07; not copied |
| gemini-1423-prompt | gemini:1423 response → gemini:1423 prompt | Gemini web (lineage), thread th_de868d9f, 2026-02-06 to 2026-02-07; not copied |
| gemini-1424-prompt | gemini:1424 response → gemini:1424 prompt | Gemini web (lineage), thread th_de868d9f, 2026-02-06 to 2026-02-07; not copied |
| gemini-1425-prompt | gemini:1425 response → gemini:1425 prompt | Gemini web (lineage), thread th_de868d9f, 2026-02-06 to 2026-02-07; not copied |
| gemini-1426-prompt | gemini:1426 response → gemini:1426 prompt | Gemini web (lineage), thread th_de868d9f, 2026-02-06 to 2026-02-07; not copied |
| gemini-1427-prompt | gemini:1427 response → gemini:1427 prompt | Gemini web (lineage), thread th_de868d9f, 2026-02-06 to 2026-02-07; not copied |
| gemini-1428-prompt | gemini:1428 response → gemini:1428 prompt | Gemini web (lineage), thread th_de868d9f, 2026-02-06 to 2026-02-07; copied: 1 pasted, 0 lifted into archive notes |
| gemini-1429-prompt | gemini:1429 response → gemini:1429 prompt | Gemini web (lineage), thread th_de868d9f, 2026-02-06 to 2026-02-07; not copied |
| gemini-1430-prompt | gemini:1430 response → gemini:1430 prompt | Gemini web (lineage), thread th_de868d9f, 2026-02-06 to 2026-02-07; not copied |
| gemini-1431-prompt | gemini:1431 response → gemini:1431 prompt | Gemini web (lineage), thread th_de868d9f, 2026-02-06 to 2026-02-07; not copied |
| gemini-2917-prompt | gemini:2917 response → gemini:2917 prompt | Gemini web (lineage), thread th_df32cc7f, 2026-03-24; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2918-prompt | gemini:2918 response → gemini:2918 prompt | Gemini web (lineage), thread th_df32cc7f, 2026-03-24; not copied |
| gemini-2959-prompt | gemini:2959 response → gemini:2959 prompt | Gemini web (lineage), thread th_df592dd2, 2026-03-25; not copied |
| gemini-2960-prompt | gemini:2960 response → gemini:2960 prompt | Gemini web (lineage), thread th_df592dd2, 2026-03-25; not copied |
| gemini-2961-prompt | gemini:2961 response → gemini:2961 prompt | Gemini web (lineage), thread th_df592dd2, 2026-03-25; not copied |
| gemini-2962-prompt | gemini:2962 response → gemini:2962 prompt | Gemini web (lineage), thread th_df592dd2, 2026-03-25; not copied |
| gemini-2963-prompt | gemini:2963 response → gemini:2963 prompt | Gemini web (lineage), thread th_df592dd2, 2026-03-25; not copied |
| gemini-2964-prompt | gemini:2964 response → gemini:2964 prompt | Gemini web (lineage), thread th_df592dd2, 2026-03-25; not copied |
| gemini-2965-prompt | gemini:2965 response → gemini:2965 prompt | Gemini web (lineage), thread th_df592dd2, 2026-03-25; not copied |
| gemini-2966-prompt | gemini:2966 response → gemini:2966 prompt | Gemini web (lineage), thread th_df592dd2, 2026-03-25; not copied |
| gemini-2967-prompt | gemini:2967 response → gemini:2967 prompt | Gemini web (lineage), thread th_df592dd2, 2026-03-25; not copied |
| gemini-2968-prompt | gemini:2968 response → gemini:2968 prompt | Gemini web (lineage), thread th_df592dd2, 2026-03-25; not copied |
| gemini-2969-prompt | gemini:2969 response → gemini:2969 prompt | Gemini web (lineage), thread th_df592dd2, 2026-03-25; not copied |
| gemini-2970-prompt | gemini:2970 response → gemini:2970 prompt | Gemini web (lineage), thread th_df592dd2, 2026-03-25; not copied |
| gemini-281-prompt | gemini:281 response → gemini:281 prompt | Gemini web (lineage), thread th_e076b4e2, 2025-12-07; copied: 6 pasted, 0 lifted into archive notes |
| gemini-284-prompt | gemini:284 response → gemini:284 prompt | Gemini web (lineage), thread th_e1567d9c, 2025-12-08; copied: 5 pasted, 0 lifted into archive notes |
| gemini-233-prompt | gemini:233 response → gemini:233 prompt | Gemini web (lineage), thread th_e1e8a741, 2025-12-05; not copied |
| gemini-2497-prompt | gemini:2497 response → gemini:2497 prompt | Gemini web (lineage), thread th_e200f009, 2026-03-06; not copied |
| gemini-11-prompt | gemini:11 response → gemini:11 prompt | Gemini web (lineage), thread th_e2b4b226, 2025-09-21; not copied |
| gemini-12-prompt | gemini:12 response → gemini:12 prompt | Gemini web (lineage), thread th_e2b4b226, 2025-09-21; not copied |
| gemini-752-prompt | gemini:752 response → gemini:752 prompt | Gemini web (lineage), thread th_e33fdc88, 2026-01-14; not copied |
| gemini-753-prompt | gemini:753 response → gemini:753 prompt | Gemini web (lineage), thread th_e33fdc88, 2026-01-14; not copied |
| gemini-754-prompt | gemini:754 response → gemini:754 prompt | Gemini web (lineage), thread th_e33fdc88, 2026-01-14; not copied |
| gemini-755-prompt | gemini:755 response → gemini:755 prompt | Gemini web (lineage), thread th_e33fdc88, 2026-01-14; not copied |
| gemini-379-prompt | gemini:379 response → gemini:379 prompt | Gemini web (lineage), thread th_e35d02d2, 2025-12-28; copied: 2 pasted, 0 lifted into archive notes |
| gemini-121-prompt | gemini:121 response → gemini:121 prompt | Gemini web (lineage), thread th_e3741369, 2025-12-02; not copied |
| gemini-3024-prompt | gemini:3024 response → gemini:3024 prompt | Gemini web (lineage), thread th_e466df6d, 2026-03-31; not copied |
| gemini-3025-prompt | gemini:3025 response → gemini:3025 prompt | Gemini web (lineage), thread th_e466df6d, 2026-03-31; not copied |
| gemini-2941-prompt | gemini:2941 response → gemini:2941 prompt | Gemini web (lineage), thread th_e4b0eb04, 2026-03-25; not copied |
| gemini-2942-prompt | gemini:2942 response → gemini:2942 prompt | Gemini web (lineage), thread th_e4b0eb04, 2026-03-25; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2943-prompt | gemini:2943 response → gemini:2943 prompt | Gemini web (lineage), thread th_e4b0eb04, 2026-03-25; not copied |
| gemini-2944-prompt | gemini:2944 response → gemini:2944 prompt | Gemini web (lineage), thread th_e4b0eb04, 2026-03-25; copied: 1 pasted, 1 lifted into archive notes |
| gemini-2945-prompt | gemini:2945 response → gemini:2945 prompt | Gemini web (lineage), thread th_e4b0eb04, 2026-03-25; not copied |
| gemini-2946-prompt | gemini:2946 response → gemini:2946 prompt | Gemini web (lineage), thread th_e4b0eb04, 2026-03-25; not copied |
| gemini-2947-prompt | gemini:2947 response → gemini:2947 prompt | Gemini web (lineage), thread th_e4b0eb04, 2026-03-25; not copied |
| gemini-2948-prompt | gemini:2948 response → gemini:2948 prompt | Gemini web (lineage), thread th_e4b0eb04, 2026-03-25; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2949-prompt | gemini:2949 response → gemini:2949 prompt | Gemini web (lineage), thread th_e4b0eb04, 2026-03-25; not copied |
| gemini-2950-prompt | gemini:2950 response → gemini:2950 prompt | Gemini web (lineage), thread th_e4b0eb04, 2026-03-25; not copied |
| gemini-1103-prompt | gemini:1103 response → gemini:1103 prompt | Gemini web (lineage), thread th_e4c02fb9, 2026-01-26; copied: 1 pasted, 0 lifted into archive notes |
| gemini-3235-prompt | gemini:3235 response → gemini:3235 prompt | Gemini web (lineage), thread th_e4e4471d, 2026-04-19; not copied |
| gemini-3073-prompt | gemini:3073 response → gemini:3073 prompt | Gemini web (lineage), thread th_e4fc3425, 2026-04-01; not copied |
| gemini-3074-prompt | gemini:3074 response → gemini:3074 prompt | Gemini web (lineage), thread th_e4fc3425, 2026-04-01; not copied |
| gemini-3075-prompt | gemini:3075 response → gemini:3075 prompt | Gemini web (lineage), thread th_e4fc3425, 2026-04-01; not copied |
| gemini-3076-prompt | gemini:3076 response → gemini:3076 prompt | Gemini web (lineage), thread th_e4fc3425, 2026-04-01; not copied |
| gemini-3077-prompt | gemini:3077 response → gemini:3077 prompt | Gemini web (lineage), thread th_e4fc3425, 2026-04-01; not copied |
| gemini-3078-prompt | gemini:3078 response → gemini:3078 prompt | Gemini web (lineage), thread th_e4fc3425, 2026-04-01; not copied |
| gemini-3079-prompt | gemini:3079 response → gemini:3079 prompt | Gemini web (lineage), thread th_e4fc3425, 2026-04-01; not copied |
| gemini-3080-prompt | gemini:3080 response → gemini:3080 prompt | Gemini web (lineage), thread th_e4fc3425, 2026-04-01; not copied |
| gemini-3081-prompt | gemini:3081 response → gemini:3081 prompt | Gemini web (lineage), thread th_e4fc3425, 2026-04-01; not copied |
| gemini-1161-prompt | gemini:1161 response → gemini:1161 prompt | Gemini web (lineage), thread th_e50c4fb1, 2026-01-27; not copied |
| gemini-1162-prompt | gemini:1162 response → gemini:1162 prompt | Gemini web (lineage), thread th_e50c4fb1, 2026-01-27; copied: 1 pasted, 0 lifted into archive notes |
| gemini-1163-prompt | gemini:1163 response → gemini:1163 prompt | Gemini web (lineage), thread th_e50c4fb1, 2026-01-27; copied: 5 pasted, 0 lifted into archive notes |
| gemini-1164-prompt | gemini:1164 response → gemini:1164 prompt | Gemini web (lineage), thread th_e50c4fb1, 2026-01-27; copied: 2 pasted, 0 lifted into archive notes |
| gemini-229-prompt | gemini:229 response → gemini:229 prompt | Gemini web (lineage), thread th_e587f968, 2025-12-05; not copied |
| gemini-1651-prompt | gemini:1651 response → gemini:1651 prompt | Gemini web (lineage), thread th_e631a3da, 2026-02-10; not copied |
| gemini-1652-prompt | gemini:1652 response → gemini:1652 prompt | Gemini web (lineage), thread th_e631a3da, 2026-02-10; not copied |
| gemini-1653-prompt | gemini:1653 response → gemini:1653 prompt | Gemini web (lineage), thread th_e631a3da, 2026-02-10; not copied |
| gemini-1654-prompt | gemini:1654 response → gemini:1654 prompt | Gemini web (lineage), thread th_e631a3da, 2026-02-10; not copied |
| gemini-1655-prompt | gemini:1655 response → gemini:1655 prompt | Gemini web (lineage), thread th_e631a3da, 2026-02-10; not copied |
| gemini-1656-prompt | gemini:1656 response → gemini:1656 prompt | Gemini web (lineage), thread th_e631a3da, 2026-02-10; not copied |
| gemini-1657-prompt | gemini:1657 response → gemini:1657 prompt | Gemini web (lineage), thread th_e631a3da, 2026-02-10; not copied |
| gemini-1658-prompt | gemini:1658 response → gemini:1658 prompt | Gemini web (lineage), thread th_e631a3da, 2026-02-10; not copied |
| gemini-1659-prompt | gemini:1659 response → gemini:1659 prompt | Gemini web (lineage), thread th_e631a3da, 2026-02-10; not copied |
| gemini-1660-prompt | gemini:1660 response → gemini:1660 prompt | Gemini web (lineage), thread th_e631a3da, 2026-02-10; not copied |
| gemini-1661-prompt | gemini:1661 response → gemini:1661 prompt | Gemini web (lineage), thread th_e631a3da, 2026-02-10; not copied |
| gemini-149-prompt | gemini:149 response → gemini:149 prompt | Gemini web (lineage), thread th_e63fe9a5, 2025-12-04; copied: 5 pasted, 0 lifted into archive notes |
| gemini-1319-prompt | gemini:1319 response → gemini:1319 prompt | Gemini web (lineage), thread th_e64f4a5f, 2026-02-04; not copied |
| gemini-1320-prompt | gemini:1320 response → gemini:1320 prompt | Gemini web (lineage), thread th_e64f4a5f, 2026-02-04; not copied |
| gemini-1321-prompt | gemini:1321 response → gemini:1321 prompt | Gemini web (lineage), thread th_e64f4a5f, 2026-02-04; not copied |
| gemini-3253-prompt | gemini:3253 response → gemini:3253 prompt | Gemini web (lineage), thread th_e66447d6, 2026-04-26; not copied |
| gemini-3254-prompt | gemini:3254 response → gemini:3254 prompt | Gemini web (lineage), thread th_e66447d6, 2026-04-26; not copied |
| gemini-3255-prompt | gemini:3255 response → gemini:3255 prompt | Gemini web (lineage), thread th_e66447d6, 2026-04-26; not copied |
| gemini-3256-prompt | gemini:3256 response → gemini:3256 prompt | Gemini web (lineage), thread th_e66447d6, 2026-04-26; not copied |
| gemini-188-prompt | gemini:188 response → gemini:188 prompt | Gemini web (lineage), thread th_e75eff4a, 2025-12-04; not copied |
| gemini-66-prompt | gemini:66 response → gemini:66 prompt | Gemini web (lineage), thread th_e7aebe09, 2025-11-26; not copied |
| gemini-3200-prompt | gemini:3200 response → gemini:3200 prompt | Gemini web (lineage), thread th_e8f58389, 2026-04-17; not copied |
| gemini-3201-prompt | gemini:3201 response → gemini:3201 prompt | Gemini web (lineage), thread th_e8f58389, 2026-04-17; not copied |
| gemini-376-prompt | gemini:376 response → gemini:376 prompt | Gemini web (lineage), thread th_e94021ef, 2025-12-28; not copied |
| gemini-1379-prompt | gemini:1379 response → gemini:1379 prompt | Gemini web (lineage), thread th_eac5c05d, 2026-02-06; not copied |
| gemini-1380-prompt | gemini:1380 response → gemini:1380 prompt | Gemini web (lineage), thread th_eac5c05d, 2026-02-06; not copied |
| gemini-1381-prompt | gemini:1381 response → gemini:1381 prompt | Gemini web (lineage), thread th_eac5c05d, 2026-02-06; not copied |
| gemini-1382-prompt | gemini:1382 response → gemini:1382 prompt | Gemini web (lineage), thread th_eac5c05d, 2026-02-06; not copied |
| gemini-1383-prompt | gemini:1383 response → gemini:1383 prompt | Gemini web (lineage), thread th_eac5c05d, 2026-02-06; not copied |
| gemini-1384-prompt | gemini:1384 response → gemini:1384 prompt | Gemini web (lineage), thread th_eac5c05d, 2026-02-06; not copied |
| gemini-1385-prompt | gemini:1385 response → gemini:1385 prompt | Gemini web (lineage), thread th_eac5c05d, 2026-02-06; not copied |
| gemini-1386-prompt | gemini:1386 response → gemini:1386 prompt | Gemini web (lineage), thread th_eac5c05d, 2026-02-06; not copied |
| gemini-1387-prompt | gemini:1387 response → gemini:1387 prompt | Gemini web (lineage), thread th_eac5c05d, 2026-02-06; not copied |
| gemini-2460-prompt | gemini:2460 response → gemini:2460 prompt | Gemini web (lineage), thread th_eada46ac, 2026-03-06; not copied |
| gemini-2461-prompt | gemini:2461 response → gemini:2461 prompt | Gemini web (lineage), thread th_eada46ac, 2026-03-06; not copied |
| gemini-2462-prompt | gemini:2462 response → gemini:2462 prompt | Gemini web (lineage), thread th_eada46ac, 2026-03-06; not copied |
| gemini-224-prompt | gemini:224 response → gemini:224 prompt | Gemini web (lineage), thread th_eaf199cc, 2025-12-05; not copied |
| gemini-2197-prompt | gemini:2197 response → gemini:2197 prompt | Gemini web (lineage), thread th_eb210d46, 2026-02-26; not copied |
| gemini-2198-prompt | gemini:2198 response → gemini:2198 prompt | Gemini web (lineage), thread th_eb210d46, 2026-02-26; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2199-prompt | gemini:2199 response → gemini:2199 prompt | Gemini web (lineage), thread th_eb210d46, 2026-02-26; not copied |
| gemini-2200-prompt | gemini:2200 response → gemini:2200 prompt | Gemini web (lineage), thread th_eb210d46, 2026-02-26; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2201-prompt | gemini:2201 response → gemini:2201 prompt | Gemini web (lineage), thread th_eb210d46, 2026-02-26; copied: 1 pasted, 0 lifted into archive notes |
| gemini-829-prompt | gemini:829 response → gemini:829 prompt | Gemini web (lineage), thread th_eb58c277, 2026-01-19; not copied |
| gemini-2223-prompt | gemini:2223 response → gemini:2223 prompt | Gemini web (lineage), thread th_eba80cb3, 2026-02-27; not copied |
| gemini-2224-prompt | gemini:2224 response → gemini:2224 prompt | Gemini web (lineage), thread th_eba80cb3, 2026-02-27; not copied |
| gemini-2225-prompt | gemini:2225 response → gemini:2225 prompt | Gemini web (lineage), thread th_eba80cb3, 2026-02-27; not copied |
| gemini-2226-prompt | gemini:2226 response → gemini:2226 prompt | Gemini web (lineage), thread th_eba80cb3, 2026-02-27; not copied |
| gemini-2227-prompt | gemini:2227 response → gemini:2227 prompt | Gemini web (lineage), thread th_eba80cb3, 2026-02-27; not copied |
| gemini-2228-prompt | gemini:2228 response → gemini:2228 prompt | Gemini web (lineage), thread th_eba80cb3, 2026-02-27; not copied |
| gemini-2229-prompt | gemini:2229 response → gemini:2229 prompt | Gemini web (lineage), thread th_eba80cb3, 2026-02-27; not copied |
| gemini-2230-prompt | gemini:2230 response → gemini:2230 prompt | Gemini web (lineage), thread th_eba80cb3, 2026-02-27; not copied |
| gemini-2343-prompt | gemini:2343 response → gemini:2343 prompt | Gemini web (lineage), thread th_eccb0e29, 2026-03-03; copied: 2 pasted, 0 lifted into archive notes |
| gemini-606-prompt | gemini:606 response → gemini:606 prompt | Gemini web (lineage), thread th_ed0a5a24, 2026-01-10; not copied |
| gemini-607-prompt | gemini:607 response → gemini:607 prompt | Gemini web (lineage), thread th_ed0a5a24, 2026-01-10; not copied |
| gemini-41-prompt | gemini:41 response → gemini:41 prompt | Gemini web (lineage), thread th_ed170c94, 2025-11-25; not copied |
| gemini-2773-prompt | gemini:2773 response → gemini:2773 prompt | Gemini web (lineage), thread th_ee3541a6, 2026-03-20; copied: 1 pasted, 0 lifted into archive notes |
| gemini-1598-prompt | gemini:1598 response → gemini:1598 prompt | Gemini web (lineage), thread th_ee5b11eb, 2026-02-10; not copied |
| gemini-1599-prompt | gemini:1599 response → gemini:1599 prompt | Gemini web (lineage), thread th_ee5b11eb, 2026-02-10; not copied |
| gemini-1600-prompt | gemini:1600 response → gemini:1600 prompt | Gemini web (lineage), thread th_ee5b11eb, 2026-02-10; not copied |
| gemini-1601-prompt | gemini:1601 response → gemini:1601 prompt | Gemini web (lineage), thread th_ee5b11eb, 2026-02-10; not copied |
| gemini-479-prompt | gemini:479 response → gemini:479 prompt | Gemini web (lineage), thread th_ee6023f6, 2026-01-08; not copied |
| gemini-480-prompt | gemini:480 response → gemini:480 prompt | Gemini web (lineage), thread th_ee6023f6, 2026-01-08; not copied |
| gemini-176-prompt | gemini:176 response → gemini:176 prompt | Gemini web (lineage), thread th_eec658b5, 2025-12-04; not copied |
| gemini-177-prompt | gemini:177 response → gemini:177 prompt | Gemini web (lineage), thread th_eec658b5, 2025-12-04; not copied |
| gemini-178-prompt | gemini:178 response → gemini:178 prompt | Gemini web (lineage), thread th_eec658b5, 2025-12-04; copied: 1 pasted, 0 lifted into archive notes |
| gemini-140-prompt | gemini:140 response → gemini:140 prompt | Gemini web (lineage), thread th_ef0c44c5, 2025-12-03; not copied |
| gemini-141-prompt | gemini:141 response → gemini:141 prompt | Gemini web (lineage), thread th_ef0c44c5, 2025-12-03; not copied |
| gemini-142-prompt | gemini:142 response → gemini:142 prompt | Gemini web (lineage), thread th_ef0c44c5, 2025-12-03; copied: 2 pasted, 1 lifted into archive notes |
| gemini-148-prompt | gemini:148 response → gemini:148 prompt | Gemini web (lineage), thread th_ef4e769d, 2025-12-04; copied: 2 pasted, 0 lifted into archive notes |
| gemini-88-prompt | gemini:88 response → gemini:88 prompt | Gemini web (lineage), thread th_ef6abcfb, 2025-12-02; copied: 1 pasted, 0 lifted into archive notes |
| gemini-89-prompt | gemini:89 response → gemini:89 prompt | Gemini web (lineage), thread th_ef6abcfb, 2025-12-02; copied: 2 pasted, 1 lifted into archive notes |
| gemini-2064-prompt | gemini:2064 response → gemini:2064 prompt | Gemini web (lineage), thread th_ef76bbc8, 2026-02-21; not copied |
| gemini-2065-prompt | gemini:2065 response → gemini:2065 prompt | Gemini web (lineage), thread th_ef76bbc8, 2026-02-21; not copied |
| gemini-2066-prompt | gemini:2066 response → gemini:2066 prompt | Gemini web (lineage), thread th_ef76bbc8, 2026-02-21; not copied |
| gemini-2067-prompt | gemini:2067 response → gemini:2067 prompt | Gemini web (lineage), thread th_ef76bbc8, 2026-02-21; not copied |
| gemini-2068-prompt | gemini:2068 response → gemini:2068 prompt | Gemini web (lineage), thread th_ef76bbc8, 2026-02-21; not copied |
| gemini-2069-prompt | gemini:2069 response → gemini:2069 prompt | Gemini web (lineage), thread th_ef76bbc8, 2026-02-21; not copied |
| gemini-2070-prompt | gemini:2070 response → gemini:2070 prompt | Gemini web (lineage), thread th_ef76bbc8, 2026-02-21; not copied |
| gemini-2071-prompt | gemini:2071 response → gemini:2071 prompt | Gemini web (lineage), thread th_ef76bbc8, 2026-02-21; not copied |
| gemini-2072-prompt | gemini:2072 response → gemini:2072 prompt | Gemini web (lineage), thread th_ef76bbc8, 2026-02-21; not copied |
| gemini-2073-prompt | gemini:2073 response → gemini:2073 prompt | Gemini web (lineage), thread th_ef76bbc8, 2026-02-21; not copied |
| gemini-2074-prompt | gemini:2074 response → gemini:2074 prompt | Gemini web (lineage), thread th_ef76bbc8, 2026-02-21; not copied |
| gemini-2849-prompt | gemini:2849 response → gemini:2849 prompt | Gemini web (lineage), thread th_efdfab46, 2026-03-22; copied: 1 pasted, 0 lifted into archive notes |
| gemini-1093-prompt | gemini:1093 response → gemini:1093 prompt | Gemini web (lineage), thread th_f00143ba, 2026-01-26; copied: 0 pasted, 1 lifted into archive notes |
| gemini-656-prompt | gemini:656 response → gemini:656 prompt | Gemini web (lineage), thread th_f0019bd7, 2026-01-12; not copied |
| gemini-657-prompt | gemini:657 response → gemini:657 prompt | Gemini web (lineage), thread th_f0019bd7, 2026-01-12; not copied |
| gemini-658-prompt | gemini:658 response → gemini:658 prompt | Gemini web (lineage), thread th_f0019bd7, 2026-01-12; not copied |
| gemini-1160-prompt | gemini:1160 response → gemini:1160 prompt | Gemini web (lineage), thread th_f0b35fec, 2026-01-27; not copied |
| gemini-3072-prompt | gemini:3072 response → gemini:3072 prompt | Gemini web (lineage), thread th_f0ece8bd, 2026-04-01; not copied |
| gemini-92-prompt | gemini:92 response → gemini:92 prompt | Gemini web (lineage), thread th_f0ee6a1d, 2025-12-02; not copied |
| gemini-3066-prompt | gemini:3066 response → gemini:3066 prompt | Gemini web (lineage), thread th_f1365992, 2026-04-01; not copied |
| gemini-3067-prompt | gemini:3067 response → gemini:3067 prompt | Gemini web (lineage), thread th_f1365992, 2026-04-01; not copied |
| gemini-3068-prompt | gemini:3068 response → gemini:3068 prompt | Gemini web (lineage), thread th_f1365992, 2026-04-01; not copied |
| gemini-3069-prompt | gemini:3069 response → gemini:3069 prompt | Gemini web (lineage), thread th_f1365992, 2026-04-01; not copied |
| gemini-3070-prompt | gemini:3070 response → gemini:3070 prompt | Gemini web (lineage), thread th_f1365992, 2026-04-01; not copied |
| gemini-3071-prompt | gemini:3071 response → gemini:3071 prompt | Gemini web (lineage), thread th_f1365992, 2026-04-01; not copied |
| gemini-2436-prompt | gemini:2436 response → gemini:2436 prompt | Gemini web (lineage), thread th_f16916a0, 2026-03-05; not copied |
| gemini-2437-prompt | gemini:2437 response → gemini:2437 prompt | Gemini web (lineage), thread th_f16916a0, 2026-03-05; not copied |
| gemini-2438-prompt | gemini:2438 response → gemini:2438 prompt | Gemini web (lineage), thread th_f16916a0, 2026-03-05; not copied |
| gemini-2439-prompt | gemini:2439 response → gemini:2439 prompt | Gemini web (lineage), thread th_f16916a0, 2026-03-05; copied: 1 pasted, 0 lifted into archive notes |
| gemini-1551-prompt | gemini:1551 response → gemini:1551 prompt | Gemini web (lineage), thread th_f2038561, 2026-02-09; not copied |
| gemini-1552-prompt | gemini:1552 response → gemini:1552 prompt | Gemini web (lineage), thread th_f2038561, 2026-02-09; not copied |
| gemini-1101-prompt | gemini:1101 response → gemini:1101 prompt | Gemini web (lineage), thread th_f20fea64, 2026-01-26; not copied |
| gemini-468-prompt | gemini:468 response → gemini:468 prompt | Gemini web (lineage), thread th_f226f558, 2026-01-08; not copied |
| gemini-469-prompt | gemini:469 response → gemini:469 prompt | Gemini web (lineage), thread th_f226f558, 2026-01-08; not copied |
| gemini-470-prompt | gemini:470 response → gemini:470 prompt | Gemini web (lineage), thread th_f226f558, 2026-01-08; not copied |
| gemini-471-prompt | gemini:471 response → gemini:471 prompt | Gemini web (lineage), thread th_f226f558, 2026-01-08; not copied |
| gemini-472-prompt | gemini:472 response → gemini:472 prompt | Gemini web (lineage), thread th_f226f558, 2026-01-08; not copied |
| gemini-473-prompt | gemini:473 response → gemini:473 prompt | Gemini web (lineage), thread th_f226f558, 2026-01-08; not copied |
| gemini-474-prompt | gemini:474 response → gemini:474 prompt | Gemini web (lineage), thread th_f226f558, 2026-01-08; not copied |
| gemini-475-prompt | gemini:475 response → gemini:475 prompt | Gemini web (lineage), thread th_f226f558, 2026-01-08; not copied |
| gemini-477-prompt | gemini:477 response → gemini:477 prompt | Gemini web (lineage), thread th_f226f558, 2026-01-08; not copied |
| gemini-256-prompt | gemini:256 response → gemini:256 prompt | Gemini web (lineage), thread th_f2dfb123, 2025-12-06; copied: 0 pasted, 2 lifted into archive notes |
| gemini-257-prompt | gemini:257 response → gemini:257 prompt | Gemini web (lineage), thread th_f2dfb123, 2025-12-06; not copied |
| gemini-1694-prompt | gemini:1694 response → gemini:1694 prompt | Gemini web (lineage), thread th_f2e8658a, 2026-02-11; not copied |
| gemini-1695-prompt | gemini:1695 response → gemini:1695 prompt | Gemini web (lineage), thread th_f2e8658a, 2026-02-11; not copied |
| gemini-1696-prompt | gemini:1696 response → gemini:1696 prompt | Gemini web (lineage), thread th_f2e8658a, 2026-02-11; not copied |
| gemini-1697-prompt | gemini:1697 response → gemini:1697 prompt | Gemini web (lineage), thread th_f2e8658a, 2026-02-11; not copied |
| gemini-2146-prompt | gemini:2146 response → gemini:2146 prompt | Gemini web (lineage), thread th_f3207f3a, 2026-02-25; not copied |
| gemini-2147-prompt | gemini:2147 response → gemini:2147 prompt | Gemini web (lineage), thread th_f3207f3a, 2026-02-25; not copied |
| gemini-2148-prompt | gemini:2148 response → gemini:2148 prompt | Gemini web (lineage), thread th_f3207f3a, 2026-02-25; not copied |
| gemini-1205-prompt | gemini:1205 response → gemini:1205 prompt | Gemini web (lineage), thread th_f3c84722, 2026-01-28; not copied |
| gemini-1206-prompt | gemini:1206 response → gemini:1206 prompt | Gemini web (lineage), thread th_f3c84722, 2026-01-28; not copied |
| gemini-1207-prompt | gemini:1207 response → gemini:1207 prompt | Gemini web (lineage), thread th_f3c84722, 2026-01-28; not copied |
| gemini-1208-prompt | gemini:1208 response → gemini:1208 prompt | Gemini web (lineage), thread th_f3c84722, 2026-01-28; copied: 4 pasted, 0 lifted into archive notes |
| gemini-2384-prompt | gemini:2384 response → gemini:2384 prompt | Gemini web (lineage), thread th_f3f8eec2, 2026-03-04; not copied |
| gemini-2385-prompt | gemini:2385 response → gemini:2385 prompt | Gemini web (lineage), thread th_f3f8eec2, 2026-03-04; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2386-prompt | gemini:2386 response → gemini:2386 prompt | Gemini web (lineage), thread th_f3f8eec2, 2026-03-04; copied: 2 pasted, 0 lifted into archive notes |
| gemini-2387-prompt | gemini:2387 response → gemini:2387 prompt | Gemini web (lineage), thread th_f3f8eec2, 2026-03-04; copied: 3 pasted, 0 lifted into archive notes |
| gemini-2388-prompt | gemini:2388 response → gemini:2388 prompt | Gemini web (lineage), thread th_f3f8eec2, 2026-03-04; not copied |
| gemini-2389-prompt | gemini:2389 response → gemini:2389 prompt | Gemini web (lineage), thread th_f3f8eec2, 2026-03-04; copied: 2 pasted, 0 lifted into archive notes |
| gemini-2390-prompt | gemini:2390 response → gemini:2390 prompt | Gemini web (lineage), thread th_f3f8eec2, 2026-03-04; not copied |
| gemini-2391-prompt | gemini:2391 response → gemini:2391 prompt | Gemini web (lineage), thread th_f3f8eec2, 2026-03-04; not copied |
| gemini-2392-prompt | gemini:2392 response → gemini:2392 prompt | Gemini web (lineage), thread th_f3f8eec2, 2026-03-04; copied: 4 pasted, 0 lifted into archive notes |
| gemini-2951-prompt | gemini:2951 response → gemini:2951 prompt | Gemini web (lineage), thread th_f40b96e8, 2026-03-25; not copied |
| gemini-2952-prompt | gemini:2952 response → gemini:2952 prompt | Gemini web (lineage), thread th_f40b96e8, 2026-03-25; not copied |
| gemini-3127-prompt | gemini:3127 response → gemini:3127 prompt | Gemini web (lineage), thread th_f454f1ae, 2026-04-08; not copied |
| gemini-3128-prompt | gemini:3128 response → gemini:3128 prompt | Gemini web (lineage), thread th_f454f1ae, 2026-04-08; not copied |
| gemini-3129-prompt | gemini:3129 response → gemini:3129 prompt | Gemini web (lineage), thread th_f454f1ae, 2026-04-08; not copied |
| gemini-3130-prompt | gemini:3130 response → gemini:3130 prompt | Gemini web (lineage), thread th_f454f1ae, 2026-04-08; not copied |
| gemini-502-prompt | gemini:502 response → gemini:502 prompt | Gemini web (lineage), thread th_f4e3faa5, 2026-01-08; not copied |
| gemini-2747-prompt | gemini:2747 response → gemini:2747 prompt | Gemini web (lineage), thread th_f54c82e4, 2026-03-17; not copied |
| gemini-2748-prompt | gemini:2748 response → gemini:2748 prompt | Gemini web (lineage), thread th_f54c82e4, 2026-03-17; not copied |
| gemini-2749-prompt | gemini:2749 response → gemini:2749 prompt | Gemini web (lineage), thread th_f54c82e4, 2026-03-17; not copied |
| gemini-2346-prompt | gemini:2346 response → gemini:2346 prompt | Gemini web (lineage), thread th_f5efbb47, 2026-03-03 to 2026-03-04; not copied |
| gemini-2347-prompt | gemini:2347 response → gemini:2347 prompt | Gemini web (lineage), thread th_f5efbb47, 2026-03-03 to 2026-03-04; not copied |
| gemini-2348-prompt | gemini:2348 response → gemini:2348 prompt | Gemini web (lineage), thread th_f5efbb47, 2026-03-03 to 2026-03-04; not copied |
| gemini-2349-prompt | gemini:2349 response → gemini:2349 prompt | Gemini web (lineage), thread th_f5efbb47, 2026-03-03 to 2026-03-04; not copied |
| gemini-2350-prompt | gemini:2350 response → gemini:2350 prompt | Gemini web (lineage), thread th_f5efbb47, 2026-03-03 to 2026-03-04; not copied |
| gemini-2351-prompt | gemini:2351 response → gemini:2351 prompt | Gemini web (lineage), thread th_f5efbb47, 2026-03-03 to 2026-03-04; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2352-prompt | gemini:2352 response → gemini:2352 prompt | Gemini web (lineage), thread th_f5efbb47, 2026-03-03 to 2026-03-04; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2353-prompt | gemini:2353 response → gemini:2353 prompt | Gemini web (lineage), thread th_f5efbb47, 2026-03-03 to 2026-03-04; not copied |
| gemini-2354-prompt | gemini:2354 response → gemini:2354 prompt | Gemini web (lineage), thread th_f5efbb47, 2026-03-03 to 2026-03-04; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2355-prompt | gemini:2355 response → gemini:2355 prompt | Gemini web (lineage), thread th_f5efbb47, 2026-03-03 to 2026-03-04; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2356-prompt | gemini:2356 response → gemini:2356 prompt | Gemini web (lineage), thread th_f5efbb47, 2026-03-03 to 2026-03-04; copied: 4 pasted, 0 lifted into archive notes |
| gemini-501-prompt | gemini:501 response → gemini:501 prompt | Gemini web (lineage), thread th_f6097b86, 2026-01-08; not copied |
| gemini-2780-prompt | gemini:2780 response → gemini:2780 prompt | Gemini web (lineage), thread th_f6216a40, 2026-03-20; copied: 2 pasted, 0 lifted into archive notes |
| gemini-2781-prompt | gemini:2781 response → gemini:2781 prompt | Gemini web (lineage), thread th_f6216a40, 2026-03-20; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2691-prompt | gemini:2691 response → gemini:2691 prompt | Gemini web (lineage), thread th_f64abd7f, 2026-03-13; not copied |
| gemini-2692-prompt | gemini:2692 response → gemini:2692 prompt | Gemini web (lineage), thread th_f64abd7f, 2026-03-13; not copied |
| gemini-2693-prompt | gemini:2693 response → gemini:2693 prompt | Gemini web (lineage), thread th_f64abd7f, 2026-03-13; not copied |
| gemini-788-prompt | gemini:788 response → gemini:788 prompt | Gemini web (lineage), thread th_f665aed1, 2026-01-17; copied: 1 pasted, 0 lifted into archive notes |
| gemini-789-prompt | gemini:789 response → gemini:789 prompt | Gemini web (lineage), thread th_f665aed1, 2026-01-17; copied: 1 pasted, 0 lifted into archive notes |
| gemini-790-prompt | gemini:790 response → gemini:790 prompt | Gemini web (lineage), thread th_f665aed1, 2026-01-17; not copied |
| gemini-227-prompt | gemini:227 response → gemini:227 prompt | Gemini web (lineage), thread th_f6b0da4b, 2025-12-05; not copied |
| gemini-228-prompt | gemini:228 response → gemini:228 prompt | Gemini web (lineage), thread th_f6b0da4b, 2025-12-05; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2396-prompt | gemini:2396 response → gemini:2396 prompt | Gemini web (lineage), thread th_f6c2e4da, 2026-03-04; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2638-prompt | gemini:2638 response → gemini:2638 prompt | Gemini web (lineage), thread th_f70829e1, 2026-03-10; copied: 2 pasted, 0 lifted into archive notes |
| gemini-2639-prompt | gemini:2639 response → gemini:2639 prompt | Gemini web (lineage), thread th_f70829e1, 2026-03-10; not copied |
| gemini-2640-prompt | gemini:2640 response → gemini:2640 prompt | Gemini web (lineage), thread th_f70829e1, 2026-03-10; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2641-prompt | gemini:2641 response → gemini:2641 prompt | Gemini web (lineage), thread th_f70829e1, 2026-03-10; not copied |
| gemini-2642-prompt | gemini:2642 response → gemini:2642 prompt | Gemini web (lineage), thread th_f70829e1, 2026-03-10; not copied |
| gemini-2643-prompt | gemini:2643 response → gemini:2643 prompt | Gemini web (lineage), thread th_f70829e1, 2026-03-10; not copied |
| gemini-2644-prompt | gemini:2644 response → gemini:2644 prompt | Gemini web (lineage), thread th_f70829e1, 2026-03-10; not copied |
| gemini-1132-prompt | gemini:1132 response → gemini:1132 prompt | Gemini web (lineage), thread th_f72d5ff8, 2026-01-27; not copied |
| gemini-1133-prompt | gemini:1133 response → gemini:1133 prompt | Gemini web (lineage), thread th_f72d5ff8, 2026-01-27; not copied |
| gemini-1134-prompt | gemini:1134 response → gemini:1134 prompt | Gemini web (lineage), thread th_f72d5ff8, 2026-01-27; not copied |
| gemini-1135-prompt | gemini:1135 response → gemini:1135 prompt | Gemini web (lineage), thread th_f72d5ff8, 2026-01-27; not copied |
| gemini-1136-prompt | gemini:1136 response → gemini:1136 prompt | Gemini web (lineage), thread th_f72d5ff8, 2026-01-27; not copied |
| gemini-1137-prompt | gemini:1137 response → gemini:1137 prompt | Gemini web (lineage), thread th_f72d5ff8, 2026-01-27; not copied |
| gemini-1138-prompt | gemini:1138 response → gemini:1138 prompt | Gemini web (lineage), thread th_f72d5ff8, 2026-01-27; not copied |
| gemini-1139-prompt | gemini:1139 response → gemini:1139 prompt | Gemini web (lineage), thread th_f72d5ff8, 2026-01-27; not copied |
| gemini-1140-prompt | gemini:1140 response → gemini:1140 prompt | Gemini web (lineage), thread th_f72d5ff8, 2026-01-27; not copied |
| gemini-1141-prompt | gemini:1141 response → gemini:1141 prompt | Gemini web (lineage), thread th_f72d5ff8, 2026-01-27; not copied |
| gemini-1142-prompt | gemini:1142 response → gemini:1142 prompt | Gemini web (lineage), thread th_f72d5ff8, 2026-01-27; not copied |
| gemini-1143-prompt | gemini:1143 response → gemini:1143 prompt | Gemini web (lineage), thread th_f72d5ff8, 2026-01-27; not copied |
| gemini-1144-prompt | gemini:1144 response → gemini:1144 prompt | Gemini web (lineage), thread th_f72d5ff8, 2026-01-27; not copied |
| gemini-1145-prompt | gemini:1145 response → gemini:1145 prompt | Gemini web (lineage), thread th_f72d5ff8, 2026-01-27; not copied |
| gemini-1146-prompt | gemini:1146 response → gemini:1146 prompt | Gemini web (lineage), thread th_f72d5ff8, 2026-01-27; not copied |
| gemini-1147-prompt | gemini:1147 response → gemini:1147 prompt | Gemini web (lineage), thread th_f72d5ff8, 2026-01-27; not copied |
| gemini-1305-prompt | gemini:1305 response → gemini:1305 prompt | Gemini web (lineage), thread th_f7481dd3, 2026-02-01; not copied |
| gemini-2971-prompt | gemini:2971 response → gemini:2971 prompt | Gemini web (lineage), thread th_f792bd77, 2026-03-25 to 2026-03-26; not copied |
| gemini-2972-prompt | gemini:2972 response → gemini:2972 prompt | Gemini web (lineage), thread th_f792bd77, 2026-03-25 to 2026-03-26; not copied |
| gemini-2973-prompt | gemini:2973 response → gemini:2973 prompt | Gemini web (lineage), thread th_f792bd77, 2026-03-25 to 2026-03-26; not copied |
| gemini-2974-prompt | gemini:2974 response → gemini:2974 prompt | Gemini web (lineage), thread th_f792bd77, 2026-03-25 to 2026-03-26; not copied |
| gemini-2975-prompt | gemini:2975 response → gemini:2975 prompt | Gemini web (lineage), thread th_f792bd77, 2026-03-25 to 2026-03-26; not copied |
| gemini-2976-prompt | gemini:2976 response → gemini:2976 prompt | Gemini web (lineage), thread th_f792bd77, 2026-03-25 to 2026-03-26; not copied |
| gemini-2977-prompt | gemini:2977 response → gemini:2977 prompt | Gemini web (lineage), thread th_f792bd77, 2026-03-25 to 2026-03-26; not copied |
| gemini-1399-prompt | gemini:1399 response → gemini:1399 prompt | Gemini web (lineage), thread th_f7abd567, 2026-02-06; copied: 2 pasted, 0 lifted into archive notes |
| gemini-1400-prompt | gemini:1400 response → gemini:1400 prompt | Gemini web (lineage), thread th_f7abd567, 2026-02-06; copied: 1 pasted, 0 lifted into archive notes |
| gemini-1401-prompt | gemini:1401 response → gemini:1401 prompt | Gemini web (lineage), thread th_f7abd567, 2026-02-06; copied: 3 pasted, 0 lifted into archive notes |
| gemini-1402-prompt | gemini:1402 response → gemini:1402 prompt | Gemini web (lineage), thread th_f7abd567, 2026-02-06; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2027-prompt | gemini:2027 response → gemini:2027 prompt | Gemini web (lineage), thread th_f810c4a7, 2026-02-21; not copied |
| gemini-2028-prompt | gemini:2028 response → gemini:2028 prompt | Gemini web (lineage), thread th_f810c4a7, 2026-02-21; not copied |
| gemini-2029-prompt | gemini:2029 response → gemini:2029 prompt | Gemini web (lineage), thread th_f810c4a7, 2026-02-21; not copied |
| gemini-2030-prompt | gemini:2030 response → gemini:2030 prompt | Gemini web (lineage), thread th_f810c4a7, 2026-02-21; not copied |
| gemini-2031-prompt | gemini:2031 response → gemini:2031 prompt | Gemini web (lineage), thread th_f810c4a7, 2026-02-21; not copied |
| gemini-2032-prompt | gemini:2032 response → gemini:2032 prompt | Gemini web (lineage), thread th_f810c4a7, 2026-02-21; not copied |
| gemini-2033-prompt | gemini:2033 response → gemini:2033 prompt | Gemini web (lineage), thread th_f810c4a7, 2026-02-21; not copied |
| gemini-2034-prompt | gemini:2034 response → gemini:2034 prompt | Gemini web (lineage), thread th_f810c4a7, 2026-02-21; not copied |
| gemini-2035-prompt | gemini:2035 response → gemini:2035 prompt | Gemini web (lineage), thread th_f810c4a7, 2026-02-21; not copied |
| gemini-2036-prompt | gemini:2036 response → gemini:2036 prompt | Gemini web (lineage), thread th_f810c4a7, 2026-02-21; not copied |
| gemini-2037-prompt | gemini:2037 response → gemini:2037 prompt | Gemini web (lineage), thread th_f810c4a7, 2026-02-21; not copied |
| gemini-2038-prompt | gemini:2038 response → gemini:2038 prompt | Gemini web (lineage), thread th_f810c4a7, 2026-02-21; not copied |
| gemini-2039-prompt | gemini:2039 response → gemini:2039 prompt | Gemini web (lineage), thread th_f810c4a7, 2026-02-21; not copied |
| gemini-2040-prompt | gemini:2040 response → gemini:2040 prompt | Gemini web (lineage), thread th_f810c4a7, 2026-02-21; not copied |
| gemini-2041-prompt | gemini:2041 response → gemini:2041 prompt | Gemini web (lineage), thread th_f810c4a7, 2026-02-21; not copied |
| gemini-2042-prompt | gemini:2042 response → gemini:2042 prompt | Gemini web (lineage), thread th_f810c4a7, 2026-02-21; not copied |
| gemini-2043-prompt | gemini:2043 response → gemini:2043 prompt | Gemini web (lineage), thread th_f810c4a7, 2026-02-21; not copied |
| gemini-2044-prompt | gemini:2044 response → gemini:2044 prompt | Gemini web (lineage), thread th_f810c4a7, 2026-02-21; not copied |
| gemini-2045-prompt | gemini:2045 response → gemini:2045 prompt | Gemini web (lineage), thread th_f810c4a7, 2026-02-21; not copied |
| gemini-2046-prompt | gemini:2046 response → gemini:2046 prompt | Gemini web (lineage), thread th_f810c4a7, 2026-02-21; not copied |
| gemini-2047-prompt | gemini:2047 response → gemini:2047 prompt | Gemini web (lineage), thread th_f810c4a7, 2026-02-21; not copied |
| gemini-2048-prompt | gemini:2048 response → gemini:2048 prompt | Gemini web (lineage), thread th_f810c4a7, 2026-02-21; not copied |
| gemini-2049-prompt | gemini:2049 response → gemini:2049 prompt | Gemini web (lineage), thread th_f810c4a7, 2026-02-21; not copied |
| gemini-2050-prompt | gemini:2050 response → gemini:2050 prompt | Gemini web (lineage), thread th_f810c4a7, 2026-02-21; not copied |
| gemini-2051-prompt | gemini:2051 response → gemini:2051 prompt | Gemini web (lineage), thread th_f810c4a7, 2026-02-21; not copied |
| gemini-2052-prompt | gemini:2052 response → gemini:2052 prompt | Gemini web (lineage), thread th_f810c4a7, 2026-02-21; not copied |
| gemini-2053-prompt | gemini:2053 response → gemini:2053 prompt | Gemini web (lineage), thread th_f810c4a7, 2026-02-21; not copied |
| gemini-2054-prompt | gemini:2054 response → gemini:2054 prompt | Gemini web (lineage), thread th_f810c4a7, 2026-02-21; not copied |
| gemini-2055-prompt | gemini:2055 response → gemini:2055 prompt | Gemini web (lineage), thread th_f810c4a7, 2026-02-21; not copied |
| gemini-2056-prompt | gemini:2056 response → gemini:2056 prompt | Gemini web (lineage), thread th_f810c4a7, 2026-02-21; not copied |
| gemini-2057-prompt | gemini:2057 response → gemini:2057 prompt | Gemini web (lineage), thread th_f810c4a7, 2026-02-21; not copied |
| gemini-2058-prompt | gemini:2058 response → gemini:2058 prompt | Gemini web (lineage), thread th_f810c4a7, 2026-02-21; not copied |
| gemini-2059-prompt | gemini:2059 response → gemini:2059 prompt | Gemini web (lineage), thread th_f810c4a7, 2026-02-21; not copied |
| gemini-2060-prompt | gemini:2060 response → gemini:2060 prompt | Gemini web (lineage), thread th_f810c4a7, 2026-02-21; not copied |
| gemini-1815-prompt | gemini:1815 response → gemini:1815 prompt | Gemini web (lineage), thread th_f8b9d479, 2026-02-13; not copied |
| gemini-1816-prompt | gemini:1816 response → gemini:1816 prompt | Gemini web (lineage), thread th_f8b9d479, 2026-02-13; not copied |
| gemini-1817-prompt | gemini:1817 response → gemini:1817 prompt | Gemini web (lineage), thread th_f8b9d479, 2026-02-13; not copied |
| gemini-1818-prompt | gemini:1818 response → gemini:1818 prompt | Gemini web (lineage), thread th_f8b9d479, 2026-02-13; not copied |
| gemini-1819-prompt | gemini:1819 response → gemini:1819 prompt | Gemini web (lineage), thread th_f8b9d479, 2026-02-13; not copied |
| gemini-1820-prompt | gemini:1820 response → gemini:1820 prompt | Gemini web (lineage), thread th_f8b9d479, 2026-02-13; not copied |
| gemini-1821-prompt | gemini:1821 response → gemini:1821 prompt | Gemini web (lineage), thread th_f8b9d479, 2026-02-13; not copied |
| gemini-1822-prompt | gemini:1822 response → gemini:1822 prompt | Gemini web (lineage), thread th_f8b9d479, 2026-02-13; not copied |
| gemini-1823-prompt | gemini:1823 response → gemini:1823 prompt | Gemini web (lineage), thread th_f8b9d479, 2026-02-13; not copied |
| gemini-1824-prompt | gemini:1824 response → gemini:1824 prompt | Gemini web (lineage), thread th_f8b9d479, 2026-02-13; not copied |
| gemini-1932-prompt | gemini:1932 response → gemini:1932 prompt | Gemini web (lineage), thread th_f8d20b5a, 2026-02-19; not copied |
| gemini-1933-prompt | gemini:1933 response → gemini:1933 prompt | Gemini web (lineage), thread th_f8d20b5a, 2026-02-19; not copied |
| gemini-1934-prompt | gemini:1934 response → gemini:1934 prompt | Gemini web (lineage), thread th_f8d20b5a, 2026-02-19; not copied |
| gemini-1935-prompt | gemini:1935 response → gemini:1935 prompt | Gemini web (lineage), thread th_f8d20b5a, 2026-02-19; not copied |
| gemini-1936-prompt | gemini:1936 response → gemini:1936 prompt | Gemini web (lineage), thread th_f8d20b5a, 2026-02-19; not copied |
| gemini-1937-prompt | gemini:1937 response → gemini:1937 prompt | Gemini web (lineage), thread th_f8d20b5a, 2026-02-19; not copied |
| gemini-1938-prompt | gemini:1938 response → gemini:1938 prompt | Gemini web (lineage), thread th_f8d20b5a, 2026-02-19; not copied |
| gemini-1939-prompt | gemini:1939 response → gemini:1939 prompt | Gemini web (lineage), thread th_f8d20b5a, 2026-02-19; not copied |
| gemini-568-prompt | gemini:568 response → gemini:568 prompt | Gemini web (lineage), thread th_f990a6b7, 2026-01-09; not copied |
| gemini-38-prompt | gemini:38 response → gemini:38 prompt | Gemini web (lineage), thread th_fa88ff1f, 2025-11-25; not copied |
| gemini-2621-prompt | gemini:2621 response → gemini:2621 prompt | Gemini web (lineage), thread th_fbe9e868, 2026-03-10; not copied |
| gemini-2622-prompt | gemini:2622 response → gemini:2622 prompt | Gemini web (lineage), thread th_fbe9e868, 2026-03-10; not copied |
| gemini-2623-prompt | gemini:2623 response → gemini:2623 prompt | Gemini web (lineage), thread th_fbe9e868, 2026-03-10; not copied |
| gemini-2624-prompt | gemini:2624 response → gemini:2624 prompt | Gemini web (lineage), thread th_fbe9e868, 2026-03-10; not copied |
| gemini-2625-prompt | gemini:2625 response → gemini:2625 prompt | Gemini web (lineage), thread th_fbe9e868, 2026-03-10; not copied |
| gemini-2626-prompt | gemini:2626 response → gemini:2626 prompt | Gemini web (lineage), thread th_fbe9e868, 2026-03-10; not copied |
| gemini-2627-prompt | gemini:2627 response → gemini:2627 prompt | Gemini web (lineage), thread th_fbe9e868, 2026-03-10; not copied |
| gemini-688-prompt | gemini:688 response → gemini:688 prompt | Gemini web (lineage), thread th_fc6370fa, 2026-01-13; not copied |
| gemini-689-prompt | gemini:689 response → gemini:689 prompt | Gemini web (lineage), thread th_fc6370fa, 2026-01-13; not copied |
| gemini-690-prompt | gemini:690 response → gemini:690 prompt | Gemini web (lineage), thread th_fc6370fa, 2026-01-13; not copied |
| gemini-691-prompt | gemini:691 response → gemini:691 prompt | Gemini web (lineage), thread th_fc6370fa, 2026-01-13; not copied |
| gemini-692-prompt | gemini:692 response → gemini:692 prompt | Gemini web (lineage), thread th_fc6370fa, 2026-01-13; not copied |
| gemini-693-prompt | gemini:693 response → gemini:693 prompt | Gemini web (lineage), thread th_fc6370fa, 2026-01-13; not copied |
| gemini-694-prompt | gemini:694 response → gemini:694 prompt | Gemini web (lineage), thread th_fc6370fa, 2026-01-13; not copied |
| gemini-695-prompt | gemini:695 response → gemini:695 prompt | Gemini web (lineage), thread th_fc6370fa, 2026-01-13; copied: 1 pasted, 0 lifted into archive notes |
| gemini-696-prompt | gemini:696 response → gemini:696 prompt | Gemini web (lineage), thread th_fc6370fa, 2026-01-13; not copied |
| gemini-105-prompt | gemini:105 response → gemini:105 prompt | Gemini web (lineage), thread th_fda050b0, 2025-12-02; not copied |
| gemini-1026-prompt | gemini:1026 response → gemini:1026 prompt | Gemini web (lineage), thread th_fdc75c5c, 2026-01-24 to 2026-01-25; not copied |
| gemini-1027-prompt | gemini:1027 response → gemini:1027 prompt | Gemini web (lineage), thread th_fdc75c5c, 2026-01-24 to 2026-01-25; not copied |
| gemini-1028-prompt | gemini:1028 response → gemini:1028 prompt | Gemini web (lineage), thread th_fdc75c5c, 2026-01-24 to 2026-01-25; not copied |
| gemini-1029-prompt | gemini:1029 response → gemini:1029 prompt | Gemini web (lineage), thread th_fdc75c5c, 2026-01-24 to 2026-01-25; not copied |
| gemini-1030-prompt | gemini:1030 response → gemini:1030 prompt | Gemini web (lineage), thread th_fdc75c5c, 2026-01-24 to 2026-01-25; not copied |
| gemini-1031-prompt | gemini:1031 response → gemini:1031 prompt | Gemini web (lineage), thread th_fdc75c5c, 2026-01-24 to 2026-01-25; not copied |
| gemini-1032-prompt | gemini:1032 response → gemini:1032 prompt | Gemini web (lineage), thread th_fdc75c5c, 2026-01-24 to 2026-01-25; not copied |
| gemini-1033-prompt | gemini:1033 response → gemini:1033 prompt | Gemini web (lineage), thread th_fdc75c5c, 2026-01-24 to 2026-01-25; not copied |
| gemini-1035-prompt | gemini:1035 response → gemini:1035 prompt | Gemini web (lineage), thread th_fdc75c5c, 2026-01-24 to 2026-01-25; not copied |
| gemini-1036-prompt | gemini:1036 response → gemini:1036 prompt | Gemini web (lineage), thread th_fdc75c5c, 2026-01-24 to 2026-01-25; not copied |
| gemini-1037-prompt | gemini:1037 response → gemini:1037 prompt | Gemini web (lineage), thread th_fdc75c5c, 2026-01-24 to 2026-01-25; not copied |
| gemini-83-prompt | gemini:83 response → gemini:83 prompt | Gemini web (lineage), thread th_fdc9ceff, 2025-12-02; copied: 0 pasted, 1 lifted into archive notes |
| gemini-25-prompt | gemini:25 response → gemini:25 prompt | Gemini web (lineage), thread th_fe0881d7, 2025-11-14; not copied |
| gemini-26-prompt | gemini:26 response → gemini:26 prompt | Gemini web (lineage), thread th_fe0881d7, 2025-11-14; not copied |
| gemini-27-prompt | gemini:27 response → gemini:27 prompt | Gemini web (lineage), thread th_fe0881d7, 2025-11-14; not copied |
| gemini-28-prompt | gemini:28 response → gemini:28 prompt | Gemini web (lineage), thread th_fe0881d7, 2025-11-14; not copied |
| gemini-402-prompt | gemini:402 response → gemini:402 prompt | Gemini web (lineage), thread th_fe607ce1, 2025-12-28; copied: 1 pasted, 0 lifted into archive notes |
| gemini-3043-prompt | gemini:3043 response → gemini:3043 prompt | Gemini web (lineage), thread th_ff3a81f0, 2026-03-31 to 2026-04-01; not copied |
| gemini-3044-prompt | gemini:3044 response → gemini:3044 prompt | Gemini web (lineage), thread th_ff3a81f0, 2026-03-31 to 2026-04-01; not copied |
| gemini-3045-prompt | gemini:3045 response → gemini:3045 prompt | Gemini web (lineage), thread th_ff3a81f0, 2026-03-31 to 2026-04-01; not copied |
| gemini-3046-prompt | gemini:3046 response → gemini:3046 prompt | Gemini web (lineage), thread th_ff3a81f0, 2026-03-31 to 2026-04-01; not copied |
| gemini-3047-prompt | gemini:3047 response → gemini:3047 prompt | Gemini web (lineage), thread th_ff3a81f0, 2026-03-31 to 2026-04-01; not copied |
| gemini-3048-prompt | gemini:3048 response → gemini:3048 prompt | Gemini web (lineage), thread th_ff3a81f0, 2026-03-31 to 2026-04-01; not copied |
| gemini-3049-prompt | gemini:3049 response → gemini:3049 prompt | Gemini web (lineage), thread th_ff3a81f0, 2026-03-31 to 2026-04-01; not copied |
| gemini-3050-prompt | gemini:3050 response → gemini:3050 prompt | Gemini web (lineage), thread th_ff3a81f0, 2026-03-31 to 2026-04-01; not copied |
| gemini-949-prompt | gemini:949 response → gemini:949 prompt | Gemini web (lineage), thread th_ff3db6bc, 2026-01-23; not copied |
| gemini-950-prompt | gemini:950 response → gemini:950 prompt | Gemini web (lineage), thread th_ff3db6bc, 2026-01-23; not copied |
| gemini-951-prompt | gemini:951 response → gemini:951 prompt | Gemini web (lineage), thread th_ff3db6bc, 2026-01-23; not copied |
| gemini-952-prompt | gemini:952 response → gemini:952 prompt | Gemini web (lineage), thread th_ff3db6bc, 2026-01-23; not copied |
| gemini-953-prompt | gemini:953 response → gemini:953 prompt | Gemini web (lineage), thread th_ff3db6bc, 2026-01-23; not copied |
| gemini-77-prompt | gemini:77 response → gemini:77 prompt | Gemini web (lineage), thread th_ff712687, 2025-12-02; not copied |
| gemini-78-prompt | gemini:78 response → gemini:78 prompt | Gemini web (lineage), thread th_ff712687, 2025-12-02; not copied |
| gemini-79-prompt | gemini:79 response → gemini:79 prompt | Gemini web (lineage), thread th_ff712687, 2025-12-02; copied: 1 pasted, 0 lifted into archive notes |
| gemini-2395-prompt | gemini:2395 response → gemini:2395 prompt | Gemini web (lineage), thread th_fff676d3, 2026-03-04; copied: 1 pasted, 0 lifted into archive notes |
| aistudio-22-t1 | aistudio:22#t3 → aistudio:22#t1 + aistudio:22#t2 | AI Studio (lineage), chat 22 "Exhaustive Cartographer", 2026-02-20; not copied |
| aistudio-22-t8 | aistudio:22#t9 → aistudio:22#t8 | AI Studio (lineage), chat 22 "Exhaustive Cartographer", 2026-02-20; not copied |
| aistudio-22-t10 | aistudio:22#t14 → aistudio:22#t10 + aistudio:22#t11 + aistudio:22#t12 + aistudio:22#t13 | AI Studio (lineage), chat 22 "Exhaustive Cartographer", 2026-02-20; not copied |
| aistudio-22-t15 | aistudio:22#t17 → aistudio:22#t15 + aistudio:22#t16 | AI Studio (lineage), chat 22 "Exhaustive Cartographer", 2026-02-20; not copied |
| aistudio-23-t1 | aistudio:23#t3 → aistudio:23#t1 + aistudio:23#t2 | AI Studio (lineage), chat 23 "Note Organizer Part 1 - Cartographer", 2026-02-20; not copied |
| aistudio-23-t8 | aistudio:23#t9 → aistudio:23#t8 | AI Studio (lineage), chat 23 "Note Organizer Part 1 - Cartographer", 2026-02-20; not copied |
| aistudio-23-t10 | aistudio:23#t14 → aistudio:23#t10 + aistudio:23#t11 + aistudio:23#t12 + aistudio:23#t13 | AI Studio (lineage), chat 23 "Note Organizer Part 1 - Cartographer", 2026-02-20; not copied |
| aistudio-23-t15 | aistudio:23#t17 → aistudio:23#t15 + aistudio:23#t16 | AI Studio (lineage), chat 23 "Note Organizer Part 1 - Cartographer", 2026-02-20; not copied |
| aistudio-24-t1 | aistudio:24#t3 → aistudio:24#t1 + aistudio:24#t2 | AI Studio (lineage), chat 24 "Note Organizer Part 2 - Sorter", 2026-02-21; not copied |
| aistudio-24-t4 | aistudio:24#t6 → aistudio:24#t4 + aistudio:24#t5 | AI Studio (lineage), chat 24 "Note Organizer Part 2 - Sorter", 2026-02-21; not copied |
| aistudio-24-t7 | aistudio:24#t9 → aistudio:24#t7 + aistudio:24#t8 | AI Studio (lineage), chat 24 "Note Organizer Part 2 - Sorter", 2026-02-21; not copied |
| aistudio-24-t10 | aistudio:24#t12 → aistudio:24#t10 + aistudio:24#t11 | AI Studio (lineage), chat 24 "Note Organizer Part 2 - Sorter", 2026-02-21; not copied |
| aistudio-24-t13 | aistudio:24#t15 → aistudio:24#t13 + aistudio:24#t14 | AI Studio (lineage), chat 24 "Note Organizer Part 2 - Sorter", 2026-02-21; not copied |
| aistudio-24-t20 | aistudio:24#t23 → aistudio:24#t20 + aistudio:24#t21 + aistudio:24#t22 | AI Studio (lineage), chat 24 "Note Organizer Part 2 - Sorter", 2026-02-21; not copied |
| aistudio-24-t24 | aistudio:24#t25 → aistudio:24#t24 | AI Studio (lineage), chat 24 "Note Organizer Part 2 - Sorter", 2026-02-21; not copied |
| aistudio-24-t26 | aistudio:24#t27 → aistudio:24#t26 | AI Studio (lineage), chat 24 "Note Organizer Part 2 - Sorter", 2026-02-21; not copied |
| aistudio-24-t28 | aistudio:24#t30 → aistudio:24#t28 + aistudio:24#t29 | AI Studio (lineage), chat 24 "Note Organizer Part 2 - Sorter", 2026-02-21; not copied |
| aistudio-24-t31 | aistudio:24#t33 → aistudio:24#t31 + aistudio:24#t32 | AI Studio (lineage), chat 24 "Note Organizer Part 2 - Sorter", 2026-02-21; not copied |
| aistudio-24-t34 | aistudio:24#t36 → aistudio:24#t34 + aistudio:24#t35 | AI Studio (lineage), chat 24 "Note Organizer Part 2 - Sorter", 2026-02-21; not copied |
| aistudio-24-t37 | aistudio:24#t39 → aistudio:24#t37 + aistudio:24#t38 | AI Studio (lineage), chat 24 "Note Organizer Part 2 - Sorter", 2026-02-21; not copied |
| aistudio-24-t40 | aistudio:24#t42 → aistudio:24#t40 + aistudio:24#t41 | AI Studio (lineage), chat 24 "Note Organizer Part 2 - Sorter", 2026-02-21; not copied |
| aistudio-25-t14 | aistudio:25#t15 → aistudio:25#t14 | AI Studio (lineage), chat 25 "Note Organizer Part 0 - Strategy Selection", 2026-02-22; not copied |
| aistudio-26-t1 | aistudio:26#t3 → aistudio:26#t1 + aistudio:26#t2 | AI Studio (lineage), chat 26 "Love Storage  Primitive To Industrial", 2026-03-26; copied: 0 pasted, 1 lifted into archive notes |
| aistudio-27-t1 | aistudio:27#t2 → aistudio:27#t1 | AI Studio (lineage), chat 27 "Fabula  Narrative Logic S Four Pillars", 2026-03-27; not copied |
| aistudio-27-t3 | aistudio:27#t5 → aistudio:27#t3 + aistudio:27#t4 | AI Studio (lineage), chat 27 "Fabula  Narrative Logic S Four Pillars", 2026-03-27; not copied |
| aistudio-27-t6 | aistudio:27#t7 → aistudio:27#t6 | AI Studio (lineage), chat 27 "Fabula  Narrative Logic S Four Pillars", 2026-03-27; not copied |
| aistudio-27-t8 | aistudio:27#t9 → aistudio:27#t8 | AI Studio (lineage), chat 27 "Fabula  Narrative Logic S Four Pillars", 2026-03-27; not copied |
| aistudio-27-t10 | aistudio:27#t11 → aistudio:27#t10 | AI Studio (lineage), chat 27 "Fabula  Narrative Logic S Four Pillars", 2026-03-27; not copied |
| aistudio-27-t12 | aistudio:27#t13 → aistudio:27#t12 | AI Studio (lineage), chat 27 "Fabula  Narrative Logic S Four Pillars", 2026-03-27; not copied |
| aistudio-27-t14 | aistudio:27#t15 → aistudio:27#t14 | AI Studio (lineage), chat 27 "Fabula  Narrative Logic S Four Pillars", 2026-03-27; not copied |
| aistudio-27-t16 | aistudio:27#t17 → aistudio:27#t16 | AI Studio (lineage), chat 27 "Fabula  Narrative Logic S Four Pillars", 2026-03-27; not copied |
| aistudio-27-t18 | aistudio:27#t19 → aistudio:27#t18 | AI Studio (lineage), chat 27 "Fabula  Narrative Logic S Four Pillars", 2026-03-27; not copied |
| aistudio-27-t20 | aistudio:27#t21 → aistudio:27#t20 | AI Studio (lineage), chat 27 "Fabula  Narrative Logic S Four Pillars", 2026-03-27; not copied |
| aistudio-27-t22 | aistudio:27#t23 → aistudio:27#t22 | AI Studio (lineage), chat 27 "Fabula  Narrative Logic S Four Pillars", 2026-03-27; not copied |
| aistudio-27-t24 | aistudio:27#t25 → aistudio:27#t24 | AI Studio (lineage), chat 27 "Fabula  Narrative Logic S Four Pillars", 2026-03-27; not copied |
| aistudio-27-t26 | aistudio:27#t27 → aistudio:27#t26 | AI Studio (lineage), chat 27 "Fabula  Narrative Logic S Four Pillars", 2026-03-27; not copied |
| aistudio-27-t28 | aistudio:27#t29 → aistudio:27#t28 | AI Studio (lineage), chat 27 "Fabula  Narrative Logic S Four Pillars", 2026-03-27; not copied |
| aistudio-28-t1 | aistudio:28#t3 → aistudio:28#t1 + aistudio:28#t2 | AI Studio (lineage), chat 28 "Workflow, Instruction, And Prompt Analysis", 2026-03-27; not copied |
| aistudio-28-t4 | aistudio:28#t5 → aistudio:28#t4 | AI Studio (lineage), chat 28 "Workflow, Instruction, And Prompt Analysis", 2026-03-27; not copied |
| aistudio-29-t1 | aistudio:29#t3 → aistudio:29#t1 + aistudio:29#t2 | AI Studio (lineage), chat 29 "Culinary Symbiosis Bridging Diet Divide", 2026-03-27; not copied |
| aistudio-29-t4 | aistudio:29#t5 → aistudio:29#t4 | AI Studio (lineage), chat 29 "Culinary Symbiosis Bridging Diet Divide", 2026-03-27; not copied |
| aistudio-29-t6 | aistudio:29#t7 → aistudio:29#t6 | AI Studio (lineage), chat 29 "Culinary Symbiosis Bridging Diet Divide", 2026-03-27; not copied |
| aistudio-29-t8 | aistudio:29#t9 → aistudio:29#t8 | AI Studio (lineage), chat 29 "Culinary Symbiosis Bridging Diet Divide", 2026-03-27; copied: 1 pasted, 0 lifted into archive notes |
| aistudio-29-t10 | aistudio:29#t11 → aistudio:29#t10 | AI Studio (lineage), chat 29 "Culinary Symbiosis Bridging Diet Divide", 2026-03-27; copied: 2 pasted, 0 lifted into archive notes |
| aistudio-30-t1 | aistudio:30#t3 → aistudio:30#t1 + aistudio:30#t2 | AI Studio (lineage), chat 30 "Uniforms  Protection Vs. Intimacy", 2026-03-28; copied: 1 pasted, 0 lifted into archive notes |
| aistudio-30-t4 | aistudio:30#t5 → aistudio:30#t4 | AI Studio (lineage), chat 30 "Uniforms  Protection Vs. Intimacy", 2026-03-28; copied: 1 pasted, 0 lifted into archive notes |
| aistudio-30-t6 | aistudio:30#t7 → aistudio:30#t6 | AI Studio (lineage), chat 30 "Uniforms  Protection Vs. Intimacy", 2026-03-28; copied: 2 pasted, 0 lifted into archive notes |
| aistudio-31-t1 | aistudio:31#t3 → aistudio:31#t1 + aistudio:31#t2 | AI Studio (lineage), chat 31 "Academy S Exclusive, Biological Requirement", 2026-03-28; not copied |
| aistudio-31-t4 | aistudio:31#t6 → aistudio:31#t4 + aistudio:31#t5 | AI Studio (lineage), chat 31 "Academy S Exclusive, Biological Requirement", 2026-03-28; copied: 1 pasted, 0 lifted into archive notes |
| aistudio-32-t1 | aistudio:32#t3 → aistudio:32#t1 + aistudio:32#t2 | AI Studio (lineage), chat 32 "Griffon Patriarchy  A Systemic Reevaluation", 2026-03-28; not copied |
| aistudio-32-t4 | aistudio:32#t5 → aistudio:32#t4 | AI Studio (lineage), chat 32 "Griffon Patriarchy  A Systemic Reevaluation", 2026-03-28; copied: 1 pasted, 0 lifted into archive notes |
| aistudio-32-t6 | aistudio:32#t7 → aistudio:32#t6 | AI Studio (lineage), chat 32 "Griffon Patriarchy  A Systemic Reevaluation", 2026-03-28; not copied |
| aistudio-32-t8 | aistudio:32#t9 → aistudio:32#t8 | AI Studio (lineage), chat 32 "Griffon Patriarchy  A Systemic Reevaluation", 2026-03-28; not copied |
| aistudio-32-t10 | aistudio:32#t11 → aistudio:32#t10 | AI Studio (lineage), chat 32 "Griffon Patriarchy  A Systemic Reevaluation", 2026-03-28; not copied |
| aistudio-32-t12 | aistudio:32#t13 → aistudio:32#t12 | AI Studio (lineage), chat 32 "Griffon Patriarchy  A Systemic Reevaluation", 2026-03-28; copied: 1 pasted, 0 lifted into archive notes |
| aistudio-33-t1 | aistudio:33#t3 → aistudio:33#t1 + aistudio:33#t2 | AI Studio (lineage), chat 33 "History Of Clothing Materials Explained", 2026-03-28; not copied |
| aistudio-34-t1 | aistudio:34#t3 → aistudio:34#t1 + aistudio:34#t2 | AI Studio (lineage), chat 34 "Trotskyist Communism, Magic Eradication, And Systemic Effects", 2026-03-28; copied: 2 pasted, 0 lifted into archive notes |
| aistudio-35-t1 | aistudio:35#t3 → aistudio:35#t1 + aistudio:35#t2 | AI Studio (lineage), chat 35 "Celestia  System Or Consensus", 2026-03-28; copied: 2 pasted, 0 lifted into archive notes |
| aistudio-36-t1 | aistudio:36#t3 → aistudio:36#t1 + aistudio:36#t2 | AI Studio (lineage), chat 36 "Skynavia S Reintegration  A Peaceful Path", 2026-03-28; copied: 1 pasted, 0 lifted into archive notes |
| aistudio-36-t4 | aistudio:36#t5 → aistudio:36#t4 | AI Studio (lineage), chat 36 "Skynavia S Reintegration  A Peaceful Path", 2026-03-28; not copied |
| aistudio-36-t6 | aistudio:36#t7 → aistudio:36#t6 | AI Studio (lineage), chat 36 "Skynavia S Reintegration  A Peaceful Path", 2026-03-28; not copied |
| aistudio-37-t1 | aistudio:37#t3 → aistudio:37#t1 + aistudio:37#t2 | AI Studio (lineage), chat 37 "The Poverty Draft S Moral Dilemma", 2026-03-28; copied: 1 pasted, 0 lifted into archive notes |
| aistudio-37-t4 | aistudio:37#t5 → aistudio:37#t4 | AI Studio (lineage), chat 37 "The Poverty Draft S Moral Dilemma", 2026-03-28; not copied |
| aistudio-38-t1 | aistudio:38#t3 → aistudio:38#t1 + aistudio:38#t2 | AI Studio (lineage), chat 38 "995 Severyanan Mutiny And Royal Guard", 2026-03-28; copied: 1 pasted, 0 lifted into archive notes |
| aistudio-39-t1 | aistudio:39#t3 → aistudio:39#t1 + aistudio:39#t2 | AI Studio (lineage), chat 39 "Star Energy  From Fertilizer To Munitions", 2026-03-28; not copied |
| aistudio-39-t4 | aistudio:39#t5 → aistudio:39#t4 | AI Studio (lineage), chat 39 "Star Energy  From Fertilizer To Munitions", 2026-03-28; copied: 1 pasted, 0 lifted into archive notes |
| aistudio-39-t6 | aistudio:39#t7 → aistudio:39#t6 | AI Studio (lineage), chat 39 "Star Energy  From Fertilizer To Munitions", 2026-03-28; copied: 2 pasted, 0 lifted into archive notes |
| aistudio-40-t1 | aistudio:40#t3 → aistudio:40#t1 + aistudio:40#t2 | AI Studio (lineage), chat 40 "Aquileian Twilight S Backstory And Magic", 2026-03-28; copied: 1 pasted, 0 lifted into archive notes |
| aistudio-40-t4 | aistudio:40#t5 → aistudio:40#t4 | AI Studio (lineage), chat 40 "Aquileian Twilight S Backstory And Magic", 2026-03-28; copied: 2 pasted, 0 lifted into archive notes |
| aistudio-40-t6 | aistudio:40#t7 → aistudio:40#t6 | AI Studio (lineage), chat 40 "Aquileian Twilight S Backstory And Magic", 2026-03-28; copied: 1 pasted, 0 lifted into archive notes |
| aistudio-41-t1 | aistudio:41#t3 → aistudio:41#t1 + aistudio:41#t2 | AI Studio (lineage), chat 41 "Eagleclaw S Marriage Refusal & Ambition", 2026-03-28; copied: 1 pasted, 0 lifted into archive notes |
| aistudio-42-t1 | aistudio:42#t3 → aistudio:42#t1 + aistudio:42#t2 | AI Studio (lineage), chat 42 "Mali S Arc  Passive To Active", 2026-03-28; not copied |
| aistudio-42-t4 | aistudio:42#t5 → aistudio:42#t4 | AI Studio (lineage), chat 42 "Mali S Arc  Passive To Active", 2026-03-28; copied: 2 pasted, 0 lifted into archive notes |
| aistudio-42-t6 | aistudio:42#t7 → aistudio:42#t6 | AI Studio (lineage), chat 42 "Mali S Arc  Passive To Active", 2026-03-28; copied: 5 pasted, 0 lifted into archive notes |
| aistudio-42-t8 | aistudio:42#t9 → aistudio:42#t8 | AI Studio (lineage), chat 42 "Mali S Arc  Passive To Active", 2026-03-28; copied: 2 pasted, 0 lifted into archive notes |
| aistudio-43-t1 | aistudio:43#t3 → aistudio:43#t1 + aistudio:43#t2 | AI Studio (lineage), chat 43 "Pinkie Pie S Resilience Explained", 2026-03-28; not copied |
| aistudio-44-t1 | aistudio:44#t3 → aistudio:44#t1 + aistudio:44#t2 | AI Studio (lineage), chat 44 "Pinkie Pie S Trauma And Deception", 2026-03-28; copied: 1 pasted, 0 lifted into archive notes |
| aistudio-44-t4 | aistudio:44#t5 → aistudio:44#t4 | AI Studio (lineage), chat 44 "Pinkie Pie S Trauma And Deception", 2026-03-28; not copied |
| aistudio-44-t6 | aistudio:44#t7 → aistudio:44#t6 | AI Studio (lineage), chat 44 "Pinkie Pie S Trauma And Deception", 2026-03-28; copied: 3 pasted, 0 lifted into archive notes |
| aistudio-44-t8 | aistudio:44#t9 → aistudio:44#t8 | AI Studio (lineage), chat 44 "Pinkie Pie S Trauma And Deception", 2026-03-28; copied: 1 pasted, 0 lifted into archive notes |
| aistudio-44-t10 | aistudio:44#t11 → aistudio:44#t10 | AI Studio (lineage), chat 44 "Pinkie Pie S Trauma And Deception", 2026-03-28; copied: 1 pasted, 1 lifted into archive notes |
| aistudio-45-t1 | aistudio:45#t3 → aistudio:45#t1 + aistudio:45#t2 | AI Studio (lineage), chat 45 "Celestia S Ideological Paralysis Explained", 2026-03-28; not copied |
| aistudio-45-t4 | aistudio:45#t5 → aistudio:45#t4 | AI Studio (lineage), chat 45 "Celestia S Ideological Paralysis Explained", 2026-03-28; copied: 1 pasted, 0 lifted into archive notes |
| aistudio-45-t6 | aistudio:45#t7 → aistudio:45#t6 | AI Studio (lineage), chat 45 "Celestia S Ideological Paralysis Explained", 2026-03-28; copied: 2 pasted, 0 lifted into archive notes |
| aistudio-45-t8 | aistudio:45#t9 → aistudio:45#t8 | AI Studio (lineage), chat 45 "Celestia S Ideological Paralysis Explained", 2026-03-28; not copied |
| aistudio-46-t1 | aistudio:46#t3 → aistudio:46#t1 + aistudio:46#t2 | AI Studio (lineage), chat 46 "Celestia S Resilience Lost, Pinkie Teaches", 2026-03-28; copied: 4 pasted, 0 lifted into archive notes |
| aistudio-46-t4 | aistudio:46#t5 → aistudio:46#t4 | AI Studio (lineage), chat 46 "Celestia S Resilience Lost, Pinkie Teaches", 2026-03-28; copied: 6 pasted, 0 lifted into archive notes |
| aistudio-46-t6 | aistudio:46#t7 → aistudio:46#t6 | AI Studio (lineage), chat 46 "Celestia S Resilience Lost, Pinkie Teaches", 2026-03-28; copied: 1 pasted, 0 lifted into archive notes |
| aistudio-47-t1 | aistudio:47#t3 → aistudio:47#t1 + aistudio:47#t2 | AI Studio (lineage), chat 47 "Velvet S Arc With Luna", 2026-03-28; not copied |
| aistudio-48-t1 | aistudio:48#t3 → aistudio:48#t1 + aistudio:48#t2 | AI Studio (lineage), chat 48 "Chrysalis S Motivations And Changeling State", 2026-03-29; not copied |
| aistudio-48-t4 | aistudio:48#t5 → aistudio:48#t4 | AI Studio (lineage), chat 48 "Chrysalis S Motivations And Changeling State", 2026-03-29; copied: 2 pasted, 0 lifted into archive notes |
| aistudio-48-t6 | aistudio:48#t7 → aistudio:48#t6 | AI Studio (lineage), chat 48 "Chrysalis S Motivations And Changeling State", 2026-03-29; copied: 2 pasted, 0 lifted into archive notes |
| aistudio-49-t1 | aistudio:49#t3 → aistudio:49#t1 + aistudio:49#t2 | AI Studio (lineage), chat 49 "Gamifying Fruit Feud For Pride", 2026-03-29; copied: 3 pasted, 0 lifted into archive notes |
| aistudio-49-t4 | aistudio:49#t5 → aistudio:49#t4 | AI Studio (lineage), chat 49 "Gamifying Fruit Feud For Pride", 2026-03-29; not copied |
| aistudio-50-t1 | aistudio:50#t3 → aistudio:50#t1 + aistudio:50#t2 | AI Studio (lineage), chat 50 "Pygmalion Complex And Domestic Deficit", 2026-03-29; copied: 3 pasted, 0 lifted into archive notes |
| aistudio-51-t1 | aistudio:51#t3 → aistudio:51#t1 + aistudio:51#t2 | AI Studio (lineage), chat 51 "Cadance, Armor, Thorax  New Alliance", 2026-03-29; not copied |
| aistudio-51-t4 | aistudio:51#t5 → aistudio:51#t4 | AI Studio (lineage), chat 51 "Cadance, Armor, Thorax  New Alliance", 2026-03-29; copied: 2 pasted, 0 lifted into archive notes |
| aistudio-52-t1 | aistudio:52#t3 → aistudio:52#t1 + aistudio:52#t2 | AI Studio (lineage), chat 52 "Critique Of Affluent Western Marxism", 2026-03-29; copied: 5 pasted, 0 lifted into archive notes |
| aistudio-52-t4 | aistudio:52#t5 → aistudio:52#t4 | AI Studio (lineage), chat 52 "Critique Of Affluent Western Marxism", 2026-03-29; copied: 5 pasted, 0 lifted into archive notes |
| aistudio-53-t1 | aistudio:53#t3 → aistudio:53#t1 + aistudio:53#t2 | AI Studio (lineage), chat 53 "Chrysalis S Geopolitical Cover-Up", 2026-03-29; copied: 3 pasted, 0 lifted into archive notes |
| aistudio-53-t4 | aistudio:53#t5 → aistudio:53#t4 | AI Studio (lineage), chat 53 "Chrysalis S Geopolitical Cover-Up", 2026-03-29; copied: 1 pasted, 0 lifted into archive notes |
| aistudio-54-t1 | aistudio:54#t3 → aistudio:54#t1 + aistudio:54#t2 | AI Studio (lineage), chat 54 "Vanhoover S Survival  Pride & Fertilizer", 2026-03-29; not copied |
| aistudio-55-t1 | aistudio:55#t3 → aistudio:55#t1 + aistudio:55#t2 | AI Studio (lineage), chat 55 "Mount Aris Trauma Divides Friends", 2026-03-29; copied: 1 pasted, 0 lifted into archive notes |
| aistudio-56-t1 | aistudio:56#t3 → aistudio:56#t1 + aistudio:56#t2 | AI Studio (lineage), chat 56 "Red Love, Magic, And Dangerous Weapon", 2026-03-29; not copied |
| aistudio-57-t1 | aistudio:57#t3 → aistudio:57#t1 + aistudio:57#t2 | AI Studio (lineage), chat 57 "Old Paradigm Remnants Found", 2026-03-29; copied: 1 pasted, 0 lifted into archive notes |
| aistudio-58-t1 | aistudio:58#t3 → aistudio:58#t1 + aistudio:58#t2 | AI Studio (lineage), chat 58 "Griffon Magic First  Apex Predators", 2026-03-29; copied: 1 pasted, 0 lifted into archive notes |
| aistudio-59-t1 | aistudio:59#t3 → aistudio:59#t1 + aistudio:59#t2 | AI Studio (lineage), chat 59 "Battle Of Britain Pilot Survival", 2026-03-29; copied: 2 pasted, 0 lifted into archive notes |
| aistudio-60-t1 | aistudio:60#t3 → aistudio:60#t1 + aistudio:60#t2 | AI Studio (lineage), chat 60 "Spike S Canon Role & Your Story", 2026-03-29; copied: 2 pasted, 0 lifted into archive notes |
| aistudio-60-t4 | aistudio:60#t5 → aistudio:60#t4 | AI Studio (lineage), chat 60 "Spike S Canon Role & Your Story", 2026-03-29; not copied |
| aistudio-60-t6 | aistudio:60#t7 → aistudio:60#t6 | AI Studio (lineage), chat 60 "Spike S Canon Role & Your Story", 2026-03-29; not copied |
| aistudio-60-t8 | aistudio:60#t9 → aistudio:60#t8 | AI Studio (lineage), chat 60 "Spike S Canon Role & Your Story", 2026-03-29; copied: 2 pasted, 0 lifted into archive notes |
| aistudio-60-t10 | aistudio:60#t11 → aistudio:60#t10 | AI Studio (lineage), chat 60 "Spike S Canon Role & Your Story", 2026-03-29; not copied |
| aistudio-60-t12 | aistudio:60#t13 → aistudio:60#t12 | AI Studio (lineage), chat 60 "Spike S Canon Role & Your Story", 2026-03-29; not copied |
| aistudio-60-t14 | aistudio:60#t15 → aistudio:60#t14 | AI Studio (lineage), chat 60 "Spike S Canon Role & Your Story", 2026-03-29; copied: 1 pasted, 0 lifted into archive notes |
| aistudio-60-t16 | aistudio:60#t17 → aistudio:60#t16 | AI Studio (lineage), chat 60 "Spike S Canon Role & Your Story", 2026-03-29; not copied |
| aistudio-60-t18 | aistudio:60#t19 → aistudio:60#t18 | AI Studio (lineage), chat 60 "Spike S Canon Role & Your Story", 2026-03-29; not copied |
| aistudio-60-t20 | aistudio:60#t21 → aistudio:60#t20 | AI Studio (lineage), chat 60 "Spike S Canon Role & Your Story", 2026-03-29; copied: 2 pasted, 0 lifted into archive notes |
| aistudio-60-t22 | aistudio:60#t23 → aistudio:60#t22 | AI Studio (lineage), chat 60 "Spike S Canon Role & Your Story", 2026-03-29; not copied |
| aistudio-60-t24 | aistudio:60#t25 → aistudio:60#t24 | AI Studio (lineage), chat 60 "Spike S Canon Role & Your Story", 2026-03-29; copied: 7 pasted, 0 lifted into archive notes |
| aistudio-60-t26 | aistudio:60#t27 → aistudio:60#t26 | AI Studio (lineage), chat 60 "Spike S Canon Role & Your Story", 2026-03-29; copied: 1 pasted, 0 lifted into archive notes |
| aistudio-60-t28 | aistudio:60#t29 → aistudio:60#t28 | AI Studio (lineage), chat 60 "Spike S Canon Role & Your Story", 2026-03-29; copied: 1 pasted, 0 lifted into archive notes |
| aistudio-60-t30 | aistudio:60#t31 → aistudio:60#t30 | AI Studio (lineage), chat 60 "Spike S Canon Role & Your Story", 2026-03-29; copied: 1 pasted, 0 lifted into archive notes |
| aistudio-60-t32 | aistudio:60#t33 → aistudio:60#t32 | AI Studio (lineage), chat 60 "Spike S Canon Role & Your Story", 2026-03-29; copied: 1 pasted, 0 lifted into archive notes |
| aistudio-60-t34 | aistudio:60#t35 → aistudio:60#t34 | AI Studio (lineage), chat 60 "Spike S Canon Role & Your Story", 2026-03-29; copied: 1 pasted, 0 lifted into archive notes |
| aistudio-60-t36 | aistudio:60#t37 → aistudio:60#t36 | AI Studio (lineage), chat 60 "Spike S Canon Role & Your Story", 2026-03-29; not copied |
| aistudio-60-t38 | aistudio:60#t39 → aistudio:60#t38 | AI Studio (lineage), chat 60 "Spike S Canon Role & Your Story", 2026-03-29; not copied |
| aistudio-60-t40 | aistudio:60#t41 → aistudio:60#t40 | AI Studio (lineage), chat 60 "Spike S Canon Role & Your Story", 2026-03-29; not copied |
| aistudio-61-t1 | aistudio:61#t3 → aistudio:61#t1 + aistudio:61#t2 | AI Studio (lineage), chat 61 "Harmonic Capitalism Fund Ideas", 2026-03-30; not copied |
| aistudio-61-t4 | aistudio:61#t5 → aistudio:61#t4 | AI Studio (lineage), chat 61 "Harmonic Capitalism Fund Ideas", 2026-03-30; not copied |
| aistudio-61-t6 | aistudio:61#t7 → aistudio:61#t6 | AI Studio (lineage), chat 61 "Harmonic Capitalism Fund Ideas", 2026-03-30; not copied |
| aistudio-61-t8 | aistudio:61#t9 → aistudio:61#t8 | AI Studio (lineage), chat 61 "Harmonic Capitalism Fund Ideas", 2026-03-30; not copied |
| aistudio-61-t10 | aistudio:61#t11 → aistudio:61#t10 | AI Studio (lineage), chat 61 "Harmonic Capitalism Fund Ideas", 2026-03-30; not copied |
| aistudio-61-t12 | aistudio:61#t13 → aistudio:61#t12 | AI Studio (lineage), chat 61 "Harmonic Capitalism Fund Ideas", 2026-03-30; not copied |
| aistudio-62-t1 | aistudio:62#t3 → aistudio:62#t1 + aistudio:62#t2 | AI Studio (lineage), chat 62 "Blueblood S Militarization  Psychological Trauma", 2026-03-31; not copied |
| aistudio-62-t4 | aistudio:62#t5 → aistudio:62#t4 | AI Studio (lineage), chat 62 "Blueblood S Militarization  Psychological Trauma", 2026-03-31; copied: 1 pasted, 0 lifted into archive notes |
| aistudio-62-t6 | aistudio:62#t7 → aistudio:62#t6 | AI Studio (lineage), chat 62 "Blueblood S Militarization  Psychological Trauma", 2026-03-31; not copied |
| aistudio-62-t8 | aistudio:62#t9 → aistudio:62#t8 | AI Studio (lineage), chat 62 "Blueblood S Militarization  Psychological Trauma", 2026-03-31; copied: 4 pasted, 0 lifted into archive notes |
| aistudio-62-t10 | aistudio:62#t11 → aistudio:62#t10 | AI Studio (lineage), chat 62 "Blueblood S Militarization  Psychological Trauma", 2026-03-31; copied: 3 pasted, 0 lifted into archive notes |
| aistudio-62-t12 | aistudio:62#t13 → aistudio:62#t12 | AI Studio (lineage), chat 62 "Blueblood S Militarization  Psychological Trauma", 2026-03-31; not copied |
| aistudio-62-t14 | aistudio:62#t15 → aistudio:62#t14 | AI Studio (lineage), chat 62 "Blueblood S Militarization  Psychological Trauma", 2026-03-31; not copied |
| aistudio-62-t16 | aistudio:62#t17 → aistudio:62#t16 | AI Studio (lineage), chat 62 "Blueblood S Militarization  Psychological Trauma", 2026-03-31; copied: 1 pasted, 0 lifted into archive notes |
| aistudio-62-t18 | aistudio:62#t19 → aistudio:62#t18 | AI Studio (lineage), chat 62 "Blueblood S Militarization  Psychological Trauma", 2026-03-31; not copied |
| aistudio-62-t20 | aistudio:62#t21 → aistudio:62#t20 | AI Studio (lineage), chat 62 "Blueblood S Militarization  Psychological Trauma", 2026-03-31; copied: 1 pasted, 0 lifted into archive notes |
| aistudio-62-t22 | aistudio:62#t23 → aistudio:62#t22 | AI Studio (lineage), chat 62 "Blueblood S Militarization  Psychological Trauma", 2026-03-31; copied: 4 pasted, 0 lifted into archive notes |
| aistudio-63-t1 | aistudio:63#t3 → aistudio:63#t1 + aistudio:63#t2 | AI Studio (lineage), chat 63 "Branch Of Blueblood S Militarization  Psychological Trauma", 2026-03-31; not copied |
| aistudio-63-t4 | aistudio:63#t5 → aistudio:63#t4 | AI Studio (lineage), chat 63 "Branch Of Blueblood S Militarization  Psychological Trauma", 2026-03-31; not copied |
| aistudio-63-t6 | aistudio:63#t7 → aistudio:63#t6 | AI Studio (lineage), chat 63 "Branch Of Blueblood S Militarization  Psychological Trauma", 2026-03-31; not copied |
| aistudio-63-t8 | aistudio:63#t9 → aistudio:63#t8 | AI Studio (lineage), chat 63 "Branch Of Blueblood S Militarization  Psychological Trauma", 2026-03-31; not copied |
| aistudio-63-t10 | aistudio:63#t11 → aistudio:63#t10 | AI Studio (lineage), chat 63 "Branch Of Blueblood S Militarization  Psychological Trauma", 2026-03-31; not copied |
| aistudio-63-t12 | aistudio:63#t13 → aistudio:63#t12 | AI Studio (lineage), chat 63 "Branch Of Blueblood S Militarization  Psychological Trauma", 2026-03-31; not copied |
| aistudio-63-t14 | aistudio:63#t15 → aistudio:63#t14 | AI Studio (lineage), chat 63 "Branch Of Blueblood S Militarization  Psychological Trauma", 2026-03-31; not copied |
| aistudio-63-t16 | aistudio:63#t17 → aistudio:63#t16 | AI Studio (lineage), chat 63 "Branch Of Blueblood S Militarization  Psychological Trauma", 2026-03-31; not copied |
| aistudio-63-t18 | aistudio:63#t19 → aistudio:63#t18 | AI Studio (lineage), chat 63 "Branch Of Blueblood S Militarization  Psychological Trauma", 2026-03-31; not copied |
| aistudio-64-t1 | aistudio:64#t3 → aistudio:64#t1 + aistudio:64#t2 | AI Studio (lineage), chat 64 "Making Fanfiction Original", 2026-04-01; not copied |
| aistudio-64-t4 | aistudio:64#t5 → aistudio:64#t4 | AI Studio (lineage), chat 64 "Making Fanfiction Original", 2026-04-01; not copied |
| aistudio-65-t1 | aistudio:65#t3 → aistudio:65#t1 + aistudio:65#t2 | AI Studio (lineage), chat 65 "Sombra S Fearful Magic Cycle", 2026-04-02; not copied |
| aistudio-65-t4 | aistudio:65#t5 → aistudio:65#t4 | AI Studio (lineage), chat 65 "Sombra S Fearful Magic Cycle", 2026-04-02; not copied |
| aistudio-65-t6 | aistudio:65#t7 → aistudio:65#t6 | AI Studio (lineage), chat 65 "Sombra S Fearful Magic Cycle", 2026-04-02; not copied |
| aistudio-65-t8 | aistudio:65#t9 → aistudio:65#t8 | AI Studio (lineage), chat 65 "Sombra S Fearful Magic Cycle", 2026-04-02; not copied |
| aistudio-66-t1 | aistudio:66#t3 → aistudio:66#t1 + aistudio:66#t2 | AI Studio (lineage), chat 66 "Velvet S Sandberg-Thatcher Synthesis", 2026-04-02; not copied |
| aistudio-67-t1 | aistudio:67#t3 → aistudio:67#t1 + aistudio:67#t2 | AI Studio (lineage), chat 67 "Branch Of Celestia S Purpose  Faust Vs. Hasbro", 2026-04-03; copied: 4 pasted, 0 lifted into archive notes |
| aistudio-67-t4 | aistudio:67#t5 → aistudio:67#t4 | AI Studio (lineage), chat 67 "Branch Of Celestia S Purpose  Faust Vs. Hasbro", 2026-04-03; copied: 1 pasted, 0 lifted into archive notes |
| aistudio-67-t6 | aistudio:67#t7 → aistudio:67#t6 | AI Studio (lineage), chat 67 "Branch Of Celestia S Purpose  Faust Vs. Hasbro", 2026-04-03; not copied |
| aistudio-67-t8 | aistudio:67#t9 → aistudio:67#t8 | AI Studio (lineage), chat 67 "Branch Of Celestia S Purpose  Faust Vs. Hasbro", 2026-04-03; copied: 4 pasted, 0 lifted into archive notes |
| aistudio-67-t10 | aistudio:67#t11 → aistudio:67#t10 | AI Studio (lineage), chat 67 "Branch Of Celestia S Purpose  Faust Vs. Hasbro", 2026-04-03; copied: 2 pasted, 1 lifted into archive notes |
| aistudio-67-t12 | aistudio:67#t13 → aistudio:67#t12 | AI Studio (lineage), chat 67 "Branch Of Celestia S Purpose  Faust Vs. Hasbro", 2026-04-03; copied: 1 pasted, 0 lifted into archive notes |
| aistudio-67-t14 | aistudio:67#t15 → aistudio:67#t14 | AI Studio (lineage), chat 67 "Branch Of Celestia S Purpose  Faust Vs. Hasbro", 2026-04-03; not copied |
| aistudio-67-t16 | aistudio:67#t17 → aistudio:67#t16 | AI Studio (lineage), chat 67 "Branch Of Celestia S Purpose  Faust Vs. Hasbro", 2026-04-03; copied: 2 pasted, 0 lifted into archive notes |
| aistudio-67-t18 | aistudio:67#t19 → aistudio:67#t18 | AI Studio (lineage), chat 67 "Branch Of Celestia S Purpose  Faust Vs. Hasbro", 2026-04-03; copied: 2 pasted, 1 lifted into archive notes |
| aistudio-67-t20 | aistudio:67#t21 → aistudio:67#t20 | AI Studio (lineage), chat 67 "Branch Of Celestia S Purpose  Faust Vs. Hasbro", 2026-04-03; copied: 2 pasted, 0 lifted into archive notes |
| aistudio-67-t22 | aistudio:67#t23 → aistudio:67#t22 | AI Studio (lineage), chat 67 "Branch Of Celestia S Purpose  Faust Vs. Hasbro", 2026-04-03; copied: 1 pasted, 0 lifted into archive notes |
| aistudio-67-t24 | aistudio:67#t25 → aistudio:67#t24 | AI Studio (lineage), chat 67 "Branch Of Celestia S Purpose  Faust Vs. Hasbro", 2026-04-03; copied: 6 pasted, 0 lifted into archive notes |
| aistudio-67-t26 | aistudio:67#t27 → aistudio:67#t26 | AI Studio (lineage), chat 67 "Branch Of Celestia S Purpose  Faust Vs. Hasbro", 2026-04-03; copied: 2 pasted, 0 lifted into archive notes |
| aistudio-67-t28 | aistudio:67#t29 → aistudio:67#t28 | AI Studio (lineage), chat 67 "Branch Of Celestia S Purpose  Faust Vs. Hasbro", 2026-04-03; not copied |
| aistudio-67-t30 | aistudio:67#t31 → aistudio:67#t30 | AI Studio (lineage), chat 67 "Branch Of Celestia S Purpose  Faust Vs. Hasbro", 2026-04-03; copied: 3 pasted, 1 lifted into archive notes |
| aistudio-67-t32 | aistudio:67#t33 → aistudio:67#t32 | AI Studio (lineage), chat 67 "Branch Of Celestia S Purpose  Faust Vs. Hasbro", 2026-04-03; not copied |
| aistudio-67-t34 | aistudio:67#t35 → aistudio:67#t34 | AI Studio (lineage), chat 67 "Branch Of Celestia S Purpose  Faust Vs. Hasbro", 2026-04-03; not copied |
| aistudio-67-t36 | aistudio:67#t37 → aistudio:67#t36 | AI Studio (lineage), chat 67 "Branch Of Celestia S Purpose  Faust Vs. Hasbro", 2026-04-03; not copied |
| aistudio-67-t38 | aistudio:67#t39 → aistudio:67#t38 | AI Studio (lineage), chat 67 "Branch Of Celestia S Purpose  Faust Vs. Hasbro", 2026-04-03; copied: 3 pasted, 0 lifted into archive notes |
| aistudio-67-t40 | aistudio:67#t41 → aistudio:67#t40 | AI Studio (lineage), chat 67 "Branch Of Celestia S Purpose  Faust Vs. Hasbro", 2026-04-03; copied: 2 pasted, 0 lifted into archive notes |
| aistudio-67-t42 | aistudio:67#t43 → aistudio:67#t42 | AI Studio (lineage), chat 67 "Branch Of Celestia S Purpose  Faust Vs. Hasbro", 2026-04-03; copied: 5 pasted, 0 lifted into archive notes |
| aistudio-67-t44 | aistudio:67#t45 → aistudio:67#t44 | AI Studio (lineage), chat 67 "Branch Of Celestia S Purpose  Faust Vs. Hasbro", 2026-04-03; not copied |
| aistudio-67-t46 | aistudio:67#t47 → aistudio:67#t46 | AI Studio (lineage), chat 67 "Branch Of Celestia S Purpose  Faust Vs. Hasbro", 2026-04-03; copied: 3 pasted, 0 lifted into archive notes |
| aistudio-67-t48 | aistudio:67#t49 → aistudio:67#t48 | AI Studio (lineage), chat 67 "Branch Of Celestia S Purpose  Faust Vs. Hasbro", 2026-04-03; copied: 2 pasted, 0 lifted into archive notes |
| aistudio-67-t50 | aistudio:67#t51 → aistudio:67#t50 | AI Studio (lineage), chat 67 "Branch Of Celestia S Purpose  Faust Vs. Hasbro", 2026-04-03; copied: 7 pasted, 0 lifted into archive notes |
| aistudio-67-t52 | aistudio:67#t53 → aistudio:67#t52 | AI Studio (lineage), chat 67 "Branch Of Celestia S Purpose  Faust Vs. Hasbro", 2026-04-03; not copied |
| aistudio-67-t54 | aistudio:67#t55 → aistudio:67#t54 | AI Studio (lineage), chat 67 "Branch Of Celestia S Purpose  Faust Vs. Hasbro", 2026-04-03; copied: 1 pasted, 0 lifted into archive notes |
| aistudio-67-t56 | aistudio:67#t57 → aistudio:67#t56 | AI Studio (lineage), chat 67 "Branch Of Celestia S Purpose  Faust Vs. Hasbro", 2026-04-03; not copied |
| aistudio-67-t58 | aistudio:67#t59 → aistudio:67#t58 | AI Studio (lineage), chat 67 "Branch Of Celestia S Purpose  Faust Vs. Hasbro", 2026-04-03; not copied |
| aistudio-67-t60 | aistudio:67#t61 → aistudio:67#t60 | AI Studio (lineage), chat 67 "Branch Of Celestia S Purpose  Faust Vs. Hasbro", 2026-04-03; copied: 1 pasted, 0 lifted into archive notes |
| aistudio-67-t62 | aistudio:67#t63 → aistudio:67#t62 | AI Studio (lineage), chat 67 "Branch Of Celestia S Purpose  Faust Vs. Hasbro", 2026-04-03; copied: 2 pasted, 0 lifted into archive notes |
| aistudio-67-t64 | aistudio:67#t65 → aistudio:67#t64 | AI Studio (lineage), chat 67 "Branch Of Celestia S Purpose  Faust Vs. Hasbro", 2026-04-03; copied: 3 pasted, 0 lifted into archive notes |
| aistudio-67-t66 | aistudio:67#t67 → aistudio:67#t66 | AI Studio (lineage), chat 67 "Branch Of Celestia S Purpose  Faust Vs. Hasbro", 2026-04-03; not copied |
| aistudio-67-t68 | aistudio:67#t69 → aistudio:67#t68 | AI Studio (lineage), chat 67 "Branch Of Celestia S Purpose  Faust Vs. Hasbro", 2026-04-03; copied: 2 pasted, 0 lifted into archive notes |
| aistudio-67-t70 | aistudio:67#t71 → aistudio:67#t70 | AI Studio (lineage), chat 67 "Branch Of Celestia S Purpose  Faust Vs. Hasbro", 2026-04-03; copied: 2 pasted, 0 lifted into archive notes |
| aistudio-67-t72 | aistudio:67#t73 → aistudio:67#t72 | AI Studio (lineage), chat 67 "Branch Of Celestia S Purpose  Faust Vs. Hasbro", 2026-04-03; copied: 2 pasted, 0 lifted into archive notes |
| aistudio-67-t74 | aistudio:67#t75 → aistudio:67#t74 | AI Studio (lineage), chat 67 "Branch Of Celestia S Purpose  Faust Vs. Hasbro", 2026-04-03; not copied |
| aistudio-67-t76 | aistudio:67#t77 → aistudio:67#t76 | AI Studio (lineage), chat 67 "Branch Of Celestia S Purpose  Faust Vs. Hasbro", 2026-04-03; copied: 5 pasted, 0 lifted into archive notes |
| aistudio-67-t78 | aistudio:67#t79 → aistudio:67#t78 | AI Studio (lineage), chat 67 "Branch Of Celestia S Purpose  Faust Vs. Hasbro", 2026-04-03; copied: 3 pasted, 0 lifted into archive notes |
| aistudio-67-t80 | aistudio:67#t81 → aistudio:67#t80 | AI Studio (lineage), chat 67 "Branch Of Celestia S Purpose  Faust Vs. Hasbro", 2026-04-03; copied: 3 pasted, 0 lifted into archive notes |
| aistudio-67-t82 | aistudio:67#t83 → aistudio:67#t82 | AI Studio (lineage), chat 67 "Branch Of Celestia S Purpose  Faust Vs. Hasbro", 2026-04-03; copied: 6 pasted, 0 lifted into archive notes |
| aistudio-67-t84 | aistudio:67#t85 → aistudio:67#t84 | AI Studio (lineage), chat 67 "Branch Of Celestia S Purpose  Faust Vs. Hasbro", 2026-04-03; copied: 2 pasted, 0 lifted into archive notes |
| aistudio-67-t86 | aistudio:67#t87 → aistudio:67#t86 | AI Studio (lineage), chat 67 "Branch Of Celestia S Purpose  Faust Vs. Hasbro", 2026-04-03; copied: 1 pasted, 0 lifted into archive notes |
| aistudio-67-t88 | aistudio:67#t89 → aistudio:67#t88 | AI Studio (lineage), chat 67 "Branch Of Celestia S Purpose  Faust Vs. Hasbro", 2026-04-03; copied: 1 pasted, 0 lifted into archive notes |
| aistudio-67-t90 | aistudio:67#t91 → aistudio:67#t90 | AI Studio (lineage), chat 67 "Branch Of Celestia S Purpose  Faust Vs. Hasbro", 2026-04-03; copied: 1 pasted, 0 lifted into archive notes |
| aistudio-67-t92 | aistudio:67#t93 → aistudio:67#t92 | AI Studio (lineage), chat 67 "Branch Of Celestia S Purpose  Faust Vs. Hasbro", 2026-04-03; not copied |
| aistudio-67-t94 | aistudio:67#t95 → aistudio:67#t94 | AI Studio (lineage), chat 67 "Branch Of Celestia S Purpose  Faust Vs. Hasbro", 2026-04-03; not copied |
| aistudio-67-t96 | aistudio:67#t97 → aistudio:67#t96 | AI Studio (lineage), chat 67 "Branch Of Celestia S Purpose  Faust Vs. Hasbro", 2026-04-03; not copied |
| aistudio-67-t98 | aistudio:67#t99 → aistudio:67#t98 | AI Studio (lineage), chat 67 "Branch Of Celestia S Purpose  Faust Vs. Hasbro", 2026-04-03; not copied |
| aistudio-67-t100 | aistudio:67#t101 → aistudio:67#t100 | AI Studio (lineage), chat 67 "Branch Of Celestia S Purpose  Faust Vs. Hasbro", 2026-04-03; copied: 4 pasted, 0 lifted into archive notes |
| aistudio-67-t102 | aistudio:67#t103 → aistudio:67#t102 | AI Studio (lineage), chat 67 "Branch Of Celestia S Purpose  Faust Vs. Hasbro", 2026-04-03; copied: 3 pasted, 0 lifted into archive notes |
| aistudio-67-t104 | aistudio:67#t105 → aistudio:67#t104 | AI Studio (lineage), chat 67 "Branch Of Celestia S Purpose  Faust Vs. Hasbro", 2026-04-03; copied: 2 pasted, 0 lifted into archive notes |
| aistudio-67-t106 | aistudio:67#t107 → aistudio:67#t106 | AI Studio (lineage), chat 67 "Branch Of Celestia S Purpose  Faust Vs. Hasbro", 2026-04-03; copied: 2 pasted, 0 lifted into archive notes |
| aistudio-67-t108 | aistudio:67#t109 → aistudio:67#t108 | AI Studio (lineage), chat 67 "Branch Of Celestia S Purpose  Faust Vs. Hasbro", 2026-04-03; copied: 2 pasted, 0 lifted into archive notes |
| aistudio-67-t110 | aistudio:67#t111 → aistudio:67#t110 | AI Studio (lineage), chat 67 "Branch Of Celestia S Purpose  Faust Vs. Hasbro", 2026-04-03; copied: 4 pasted, 0 lifted into archive notes |
| aistudio-67-t112 | aistudio:67#t113 → aistudio:67#t112 | AI Studio (lineage), chat 67 "Branch Of Celestia S Purpose  Faust Vs. Hasbro", 2026-04-03; copied: 2 pasted, 0 lifted into archive notes |
| aistudio-67-t114 | aistudio:67#t115 → aistudio:67#t114 | AI Studio (lineage), chat 67 "Branch Of Celestia S Purpose  Faust Vs. Hasbro", 2026-04-03; copied: 5 pasted, 0 lifted into archive notes |
| aistudio-67-t116 | aistudio:67#t117 → aistudio:67#t116 | AI Studio (lineage), chat 67 "Branch Of Celestia S Purpose  Faust Vs. Hasbro", 2026-04-03; not copied |
| aistudio-67-t118 | aistudio:67#t119 → aistudio:67#t118 | AI Studio (lineage), chat 67 "Branch Of Celestia S Purpose  Faust Vs. Hasbro", 2026-04-03; not copied |
| aistudio-67-t120 | aistudio:67#t121 → aistudio:67#t120 | AI Studio (lineage), chat 67 "Branch Of Celestia S Purpose  Faust Vs. Hasbro", 2026-04-03; not copied |
| aistudio-68-t1 | aistudio:68#t3 → aistudio:68#t1 + aistudio:68#t2 | AI Studio (lineage), chat 68 "Celestia S Purpose  Faust Vs. Hasbro", 2026-04-03; not copied |
| aistudio-68-t4 | aistudio:68#t5 → aistudio:68#t4 | AI Studio (lineage), chat 68 "Celestia S Purpose  Faust Vs. Hasbro", 2026-04-03; not copied |
| aistudio-68-t6 | aistudio:68#t7 → aistudio:68#t6 | AI Studio (lineage), chat 68 "Celestia S Purpose  Faust Vs. Hasbro", 2026-04-03; not copied |
| aistudio-68-t8 | aistudio:68#t9 → aistudio:68#t8 | AI Studio (lineage), chat 68 "Celestia S Purpose  Faust Vs. Hasbro", 2026-04-03; not copied |
| aistudio-68-t10 | aistudio:68#t11 → aistudio:68#t10 | AI Studio (lineage), chat 68 "Celestia S Purpose  Faust Vs. Hasbro", 2026-04-03; not copied |
| aistudio-68-t12 | aistudio:68#t13 → aistudio:68#t12 | AI Studio (lineage), chat 68 "Celestia S Purpose  Faust Vs. Hasbro", 2026-04-03; not copied |
| aistudio-68-t14 | aistudio:68#t15 → aistudio:68#t14 | AI Studio (lineage), chat 68 "Celestia S Purpose  Faust Vs. Hasbro", 2026-04-03; not copied |
| aistudio-68-t16 | aistudio:68#t17 → aistudio:68#t16 | AI Studio (lineage), chat 68 "Celestia S Purpose  Faust Vs. Hasbro", 2026-04-03; not copied |
| aistudio-68-t18 | aistudio:68#t19 → aistudio:68#t18 | AI Studio (lineage), chat 68 "Celestia S Purpose  Faust Vs. Hasbro", 2026-04-03; not copied |
| aistudio-68-t20 | aistudio:68#t21 → aistudio:68#t20 | AI Studio (lineage), chat 68 "Celestia S Purpose  Faust Vs. Hasbro", 2026-04-03; not copied |
| aistudio-68-t22 | aistudio:68#t23 → aistudio:68#t22 | AI Studio (lineage), chat 68 "Celestia S Purpose  Faust Vs. Hasbro", 2026-04-03; not copied |
| aistudio-68-t24 | aistudio:68#t25 → aistudio:68#t24 | AI Studio (lineage), chat 68 "Celestia S Purpose  Faust Vs. Hasbro", 2026-04-03; not copied |
| aistudio-68-t26 | aistudio:68#t27 → aistudio:68#t26 | AI Studio (lineage), chat 68 "Celestia S Purpose  Faust Vs. Hasbro", 2026-04-03; not copied |
| aistudio-68-t28 | aistudio:68#t29 → aistudio:68#t28 | AI Studio (lineage), chat 68 "Celestia S Purpose  Faust Vs. Hasbro", 2026-04-03; not copied |
| aistudio-68-t30 | aistudio:68#t31 → aistudio:68#t30 | AI Studio (lineage), chat 68 "Celestia S Purpose  Faust Vs. Hasbro", 2026-04-03; not copied |
| aistudio-68-t32 | aistudio:68#t33 → aistudio:68#t32 | AI Studio (lineage), chat 68 "Celestia S Purpose  Faust Vs. Hasbro", 2026-04-03; not copied |
| aistudio-68-t34 | aistudio:68#t35 → aistudio:68#t34 | AI Studio (lineage), chat 68 "Celestia S Purpose  Faust Vs. Hasbro", 2026-04-03; not copied |
| aistudio-68-t36 | aistudio:68#t37 → aistudio:68#t36 | AI Studio (lineage), chat 68 "Celestia S Purpose  Faust Vs. Hasbro", 2026-04-03; not copied |
| aistudio-68-t38 | aistudio:68#t39 → aistudio:68#t38 | AI Studio (lineage), chat 68 "Celestia S Purpose  Faust Vs. Hasbro", 2026-04-03; not copied |
| aistudio-68-t40 | aistudio:68#t41 → aistudio:68#t40 | AI Studio (lineage), chat 68 "Celestia S Purpose  Faust Vs. Hasbro", 2026-04-03; not copied |
| aistudio-69-t1 | aistudio:69#t3 → aistudio:69#t1 + aistudio:69#t2 | AI Studio (lineage), chat 69 "Tantabus  Psychological Heat Sink", 2026-04-04; not copied |
| aistudio-69-t4 | aistudio:69#t5 → aistudio:69#t4 | AI Studio (lineage), chat 69 "Tantabus  Psychological Heat Sink", 2026-04-04; not copied |
| aistudio-69-t6 | aistudio:69#t7 → aistudio:69#t6 | AI Studio (lineage), chat 69 "Tantabus  Psychological Heat Sink", 2026-04-04; not copied |
| aistudio-70-t1 | aistudio:70#t3 → aistudio:70#t1 + aistudio:70#t2 | AI Studio (lineage), chat 70 "Manehattan S Recall Mechanics  Two Options", 2026-04-04; not copied |
| aistudio-70-t4 | aistudio:70#t5 → aistudio:70#t4 | AI Studio (lineage), chat 70 "Manehattan S Recall Mechanics  Two Options", 2026-04-04; not copied |
| aistudio-70-t6 | aistudio:70#t7 → aistudio:70#t6 | AI Studio (lineage), chat 70 "Manehattan S Recall Mechanics  Two Options", 2026-04-04; copied: 3 pasted, 0 lifted into archive notes |
| aistudio-71-t1 | aistudio:71#t3 → aistudio:71#t1 + aistudio:71#t2 | AI Studio (lineage), chat 71 "Generosity S Tyranny  Extraction", 2026-04-04; not copied |
| aistudio-71-t4 | aistudio:71#t5 → aistudio:71#t4 | AI Studio (lineage), chat 71 "Generosity S Tyranny  Extraction", 2026-04-04; not copied |
| aistudio-71-t6 | aistudio:71#t7 → aistudio:71#t6 | AI Studio (lineage), chat 71 "Generosity S Tyranny  Extraction", 2026-04-04; not copied |
| aistudio-72-t1 | aistudio:72#t3 → aistudio:72#t1 + aistudio:72#t2 | AI Studio (lineage), chat 72 "Authenticity Vs. Poseur Branding", 2026-04-04; not copied |
| aistudio-72-t4 | aistudio:72#t5 → aistudio:72#t4 | AI Studio (lineage), chat 72 "Authenticity Vs. Poseur Branding", 2026-04-04; not copied |
| aistudio-73-t1 | aistudio:73#t3 → aistudio:73#t1 + aistudio:73#t2 | AI Studio (lineage), chat 73 "Supremacy  The Dark Magic Mirror", 2026-04-04; copied: 1 pasted, 0 lifted into archive notes |
| aistudio-74-t1 | aistudio:74#t3 → aistudio:74#t1 + aistudio:74#t2 | AI Studio (lineage), chat 74 "Bottom-Up   Top-Down Strengthened", 2026-04-04; copied: 3 pasted, 0 lifted into archive notes |
| aistudio-75-t1 | aistudio:75#t3 → aistudio:75#t1 + aistudio:75#t2 | AI Studio (lineage), chat 75 "Applejack S Trauma And Ideological Protest", 2026-04-04; not copied |
| aistudio-76-t1 | aistudio:76#t3 → aistudio:76#t1 + aistudio:76#t2 | AI Studio (lineage), chat 76 "Applejack S Element And Statecraft", 2026-04-04; not copied |
| aistudio-76-t4 | aistudio:76#t5 → aistudio:76#t4 | AI Studio (lineage), chat 76 "Applejack S Element And Statecraft", 2026-04-04; not copied |
| aistudio-76-t6 | aistudio:76#t7 → aistudio:76#t6 | AI Studio (lineage), chat 76 "Applejack S Element And Statecraft", 2026-04-04; copied: 1 pasted, 0 lifted into archive notes |
| aistudio-77-t1 | aistudio:77#t3 → aistudio:77#t1 + aistudio:77#t2 | AI Studio (lineage), chat 77 "Structuring Your Fanfiction Narrative", 2026-04-05; not copied |
| aistudio-78-t1 | aistudio:78#t3 → aistudio:78#t1 + aistudio:78#t2 | AI Studio (lineage), chat 78 "Twilight S Mask And Celestia S Expectations", 2026-04-05; not copied |
| aistudio-79-t1 | aistudio:79#t3 → aistudio:79#t1 + aistudio:79#t2 | AI Studio (lineage), chat 79 "Fabula Architectural Evolution Summary", 2026-04-06; not copied |
| aistudio-80-t1 | aistudio:80#t4 → aistudio:80#t1 + aistudio:80#t2 + aistudio:80#t3 | AI Studio (lineage), chat 80 "Severyanan Industrialism S Bessemer Boon", 2026-04-06; copied: 2 pasted, 0 lifted into archive notes |
| aistudio-80-t5 | aistudio:80#t6 → aistudio:80#t5 | AI Studio (lineage), chat 80 "Severyanan Industrialism S Bessemer Boon", 2026-04-06; copied: 7 pasted, 0 lifted into archive notes |
| aistudio-80-t7 | aistudio:80#t8 → aistudio:80#t7 | AI Studio (lineage), chat 80 "Severyanan Industrialism S Bessemer Boon", 2026-04-06; not copied |
| aistudio-80-t9 | aistudio:80#t10 → aistudio:80#t9 | AI Studio (lineage), chat 80 "Severyanan Industrialism S Bessemer Boon", 2026-04-06; copied: 5 pasted, 0 lifted into archive notes |
| aistudio-80-t11 | aistudio:80#t12 → aistudio:80#t11 | AI Studio (lineage), chat 80 "Severyanan Industrialism S Bessemer Boon", 2026-04-06; copied: 6 pasted, 0 lifted into archive notes |
| aistudio-80-t13 | aistudio:80#t14 → aistudio:80#t13 | AI Studio (lineage), chat 80 "Severyanan Industrialism S Bessemer Boon", 2026-04-06; not copied |
| aistudio-80-t15 | aistudio:80#t16 → aistudio:80#t15 | AI Studio (lineage), chat 80 "Severyanan Industrialism S Bessemer Boon", 2026-04-06; copied: 2 pasted, 0 lifted into archive notes |
| aistudio-81-t1 | aistudio:81#t3 → aistudio:81#t1 + aistudio:81#t2 | AI Studio (lineage), chat 81 "Player Generals Versus Ai", 2026-04-07; not copied |
| aistudio-82-t1 | aistudio:82#t3 → aistudio:82#t1 + aistudio:82#t2 | AI Studio (lineage), chat 82 "Old Paradigm Vs. New Paradigm", 2026-04-09; not copied |
| aistudio-82-t4 | aistudio:82#t5 → aistudio:82#t4 | AI Studio (lineage), chat 82 "Old Paradigm Vs. New Paradigm", 2026-04-09; copied: 2 pasted, 0 lifted into archive notes |
| aistudio-82-t6 | aistudio:82#t7 → aistudio:82#t6 | AI Studio (lineage), chat 82 "Old Paradigm Vs. New Paradigm", 2026-04-09; copied: 3 pasted, 0 lifted into archive notes |
| aistudio-82-t8 | aistudio:82#t9 → aistudio:82#t8 | AI Studio (lineage), chat 82 "Old Paradigm Vs. New Paradigm", 2026-04-09; copied: 1 pasted, 0 lifted into archive notes |
| aistudio-82-t10 | aistudio:82#t11 → aistudio:82#t10 | AI Studio (lineage), chat 82 "Old Paradigm Vs. New Paradigm", 2026-04-09; not copied |
| aistudio-83-t1 | aistudio:83#t3 → aistudio:83#t1 + aistudio:83#t2 | AI Studio (lineage), chat 83 "Story Plan Contradictions And Corrections", 2026-04-09; copied: 2 pasted, 0 lifted into archive notes |
| aistudio-84-t1 | aistudio:84#t3 → aistudio:84#t1 + aistudio:84#t2 | AI Studio (lineage), chat 84 "President Applejack S Peacetime Element", 2026-04-10; not copied |
| aistudio-84-t4 | aistudio:84#t5 → aistudio:84#t4 | AI Studio (lineage), chat 84 "President Applejack S Peacetime Element", 2026-04-10; not copied |
| aistudio-84-t6 | aistudio:84#t7 → aistudio:84#t6 | AI Studio (lineage), chat 84 "President Applejack S Peacetime Element", 2026-04-10; not copied |
| aistudio-84-t8 | aistudio:84#t9 → aistudio:84#t8 | AI Studio (lineage), chat 84 "President Applejack S Peacetime Element", 2026-04-10; not copied |
| aistudio-84-t10 | aistudio:84#t11 → aistudio:84#t10 | AI Studio (lineage), chat 84 "President Applejack S Peacetime Element", 2026-04-10; not copied |
| aistudio-85-t1 | aistudio:85#t3 → aistudio:85#t1 + aistudio:85#t2 | AI Studio (lineage), chat 85 "Literal Translator Mechanics Explained", 2026-04-12; not copied |
| aistudio-85-t4 | aistudio:85#t5 → aistudio:85#t4 | AI Studio (lineage), chat 85 "Literal Translator Mechanics Explained", 2026-04-12; not copied |
| aistudio-85-t6 | aistudio:85#t7 → aistudio:85#t6 | AI Studio (lineage), chat 85 "Literal Translator Mechanics Explained", 2026-04-12; not copied |
| aistudio-85-t8 | aistudio:85#t9 → aistudio:85#t8 | AI Studio (lineage), chat 85 "Literal Translator Mechanics Explained", 2026-04-12; not copied |
| aistudio-85-t10 | aistudio:85#t11 → aistudio:85#t10 | AI Studio (lineage), chat 85 "Literal Translator Mechanics Explained", 2026-04-12; not copied |
| aistudio-86-t1 | aistudio:86#t3 → aistudio:86#t1 + aistudio:86#t2 | AI Studio (lineage), chat 86 "Jaeger-Geist S Toxic Positivity Cage", 2026-04-13; copied: 2 pasted, 0 lifted into archive notes |
| aistudio-86-t4 | aistudio:86#t5 → aistudio:86#t4 | AI Studio (lineage), chat 86 "Jaeger-Geist S Toxic Positivity Cage", 2026-04-13; not copied |
| aistudio-86-t6 | aistudio:86#t7 → aistudio:86#t6 | AI Studio (lineage), chat 86 "Jaeger-Geist S Toxic Positivity Cage", 2026-04-13; not copied |
| aistudio-86-t8 | aistudio:86#t9 → aistudio:86#t8 | AI Studio (lineage), chat 86 "Jaeger-Geist S Toxic Positivity Cage", 2026-04-13; copied: 1 pasted, 0 lifted into archive notes |
| aistudio-87-t1 | aistudio:87#t3 → aistudio:87#t1 + aistudio:87#t2 | AI Studio (lineage), chat 87 "Chrysalis S Weaponized Love", 2026-04-13; not copied |
| aistudio-87-t4 | aistudio:87#t5 → aistudio:87#t4 | AI Studio (lineage), chat 87 "Chrysalis S Weaponized Love", 2026-04-13; copied: 1 pasted, 0 lifted into archive notes |
| aistudio-87-t6 | aistudio:87#t7 → aistudio:87#t6 | AI Studio (lineage), chat 87 "Chrysalis S Weaponized Love", 2026-04-13; not copied |
| aistudio-88-t1 | aistudio:88#t3 → aistudio:88#t1 + aistudio:88#t2 | AI Studio (lineage), chat 88 "Cutie Marks  Magic, Talent, And Destiny", 2026-04-13; copied: 1 pasted, 0 lifted into archive notes |
| aistudio-89-t1 | aistudio:89#t3 → aistudio:89#t1 + aistudio:89#t2 | AI Studio (lineage), chat 89 "Flurry Heart S Alicorn Birth Explained", 2026-04-13; copied: 1 pasted, 0 lifted into archive notes |
| aistudio-90-t1 | aistudio:90#t3 → aistudio:90#t1 + aistudio:90#t2 | AI Studio (lineage), chat 90 "Temberik Isolation Vs. Tzinacatl Stagnation", 2026-04-13; not copied |
| aistudio-91-t1 | aistudio:91#t3 → aistudio:91#t1 + aistudio:91#t2 | AI Studio (lineage), chat 91 "Applejack S Radio  Doctrinal Failure S Symbol", 2026-04-14; not copied |
| aistudio-91-t4 | aistudio:91#t5 → aistudio:91#t4 | AI Studio (lineage), chat 91 "Applejack S Radio  Doctrinal Failure S Symbol", 2026-04-14; not copied |
| aistudio-92-t1 | aistudio:92#t3 → aistudio:92#t1 + aistudio:92#t2 | AI Studio (lineage), chat 92 "Equestrian Sex Reform  Ideological Terror", 2026-04-14; not copied |
| aistudio-92-t4 | aistudio:92#t5 → aistudio:92#t4 | AI Studio (lineage), chat 92 "Equestrian Sex Reform  Ideological Terror", 2026-04-14; copied: 1 pasted, 0 lifted into archive notes |
| aistudio-93-t1 | aistudio:93#t3 → aistudio:93#t1 + aistudio:93#t2 | AI Studio (lineage), chat 93 "Cadance S Secret Magic Transfer Plan", 2026-04-14; not copied |
| aistudio-93-t4 | aistudio:93#t5 → aistudio:93#t4 | AI Studio (lineage), chat 93 "Cadance S Secret Magic Transfer Plan", 2026-04-14; not copied |
| aistudio-94-t1 | aistudio:94#t3 → aistudio:94#t1 + aistudio:94#t2 | AI Studio (lineage), chat 94 "Surrogate For Equestrian Magic Bafflement", 2026-04-14; not copied |
| aistudio-94-t4 | aistudio:94#t5 → aistudio:94#t4 | AI Studio (lineage), chat 94 "Surrogate For Equestrian Magic Bafflement", 2026-04-14; not copied |
| aistudio-94-t6 | aistudio:94#t7 → aistudio:94#t6 | AI Studio (lineage), chat 94 "Surrogate For Equestrian Magic Bafflement", 2026-04-14; not copied |
| aistudio-94-t8 | aistudio:94#t9 → aistudio:94#t8 | AI Studio (lineage), chat 94 "Surrogate For Equestrian Magic Bafflement", 2026-04-14; not copied |
| aistudio-95-t1 | aistudio:95#t6 → aistudio:95#t1 + aistudio:95#t2 + aistudio:95#t3 + aistudio:95#t4 + aistudio:95#t5 | AI Studio (lineage), chat 95 "Story Planner Organization And Fabula Structuring", 2026-04-15; not copied |
| nlm-2-t1 | nlm:2#t2 → nlm:2#t1 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; not copied |
| nlm-2-t3 | nlm:2#t4 → nlm:2#t3 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; not copied |
| nlm-2-t5 | nlm:2#t6 → nlm:2#t5 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; not copied |
| nlm-2-t7 | nlm:2#t8 → nlm:2#t7 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; not copied |
| nlm-2-t9 | nlm:2#t10 → nlm:2#t9 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; not copied |
| nlm-2-t11 | nlm:2#t12 → nlm:2#t11 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; not copied |
| nlm-2-t13 | nlm:2#t14 → nlm:2#t13 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; not copied |
| nlm-2-t15 | nlm:2#t16 → nlm:2#t15 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; not copied |
| nlm-2-t17 | nlm:2#t18 → nlm:2#t17 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; not copied |
| nlm-2-t19 | nlm:2#t20 → nlm:2#t19 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; not copied |
| nlm-2-t21 | nlm:2#t22 → nlm:2#t21 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; not copied |
| nlm-2-t23 | nlm:2#t24 → nlm:2#t23 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; not copied |
| nlm-2-t25 | nlm:2#t26 → nlm:2#t25 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; not copied |
| nlm-2-t27 | nlm:2#t28 → nlm:2#t27 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; copied: 0 pasted, 1 lifted into archive notes |
| nlm-2-t29 | nlm:2#t30 → nlm:2#t29 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; copied: 1 pasted, 0 lifted into archive notes |
| nlm-2-t31 | nlm:2#t32 → nlm:2#t31 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; copied: 1 pasted, 0 lifted into archive notes |
| nlm-2-t33 | nlm:2#t34 → nlm:2#t33 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; not copied |
| nlm-2-t35 | nlm:2#t36 → nlm:2#t35 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; not copied |
| nlm-2-t37 | nlm:2#t38 → nlm:2#t37 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; not copied |
| nlm-2-t39 | nlm:2#t40 → nlm:2#t39 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; not copied |
| nlm-2-t41 | nlm:2#t42 → nlm:2#t41 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; not copied |
| nlm-2-t43 | nlm:2#t44 → nlm:2#t43 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; not copied |
| nlm-2-t45 | nlm:2#t46 → nlm:2#t45 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; not copied |
| nlm-2-t47 | nlm:2#t48 → nlm:2#t47 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; not copied |
| nlm-2-t49 | nlm:2#t50 → nlm:2#t49 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; not copied |
| nlm-2-t51 | nlm:2#t52 → nlm:2#t51 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; not copied |
| nlm-2-t53 | nlm:2#t54 → nlm:2#t53 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; copied: 0 pasted, 1 lifted into archive notes |
| nlm-2-t55 | nlm:2#t56 → nlm:2#t55 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; not copied |
| nlm-2-t57 | nlm:2#t58 → nlm:2#t57 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; not copied |
| nlm-2-t59 | nlm:2#t60 → nlm:2#t59 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; not copied |
| nlm-2-t61 | nlm:2#t62 → nlm:2#t61 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; copied: 1 pasted, 1 lifted into archive notes |
| nlm-2-t63 | nlm:2#t64 → nlm:2#t63 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; not copied |
| nlm-2-t65 | nlm:2#t66 → nlm:2#t65 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; not copied |
| nlm-2-t67 | nlm:2#t68 → nlm:2#t67 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; not copied |
| nlm-2-t69 | nlm:2#t70 → nlm:2#t69 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; not copied |
| nlm-2-t71 | nlm:2#t72 → nlm:2#t71 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; not copied |
| nlm-2-t73 | nlm:2#t74 → nlm:2#t73 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; copied: 0 pasted, 1 lifted into archive notes |
| nlm-2-t75 | nlm:2#t76 → nlm:2#t75 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; not copied |
| nlm-2-t77 | nlm:2#t78 → nlm:2#t77 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; copied: 0 pasted, 1 lifted into archive notes |
| nlm-2-t79 | nlm:2#t80 → nlm:2#t79 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; not copied |
| nlm-2-t81 | nlm:2#t82 → nlm:2#t81 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; not copied |
| nlm-2-t83 | nlm:2#t84 → nlm:2#t83 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; not copied |
| nlm-2-t85 | nlm:2#t86 → nlm:2#t85 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; not copied |
| nlm-2-t87 | nlm:2#t88 → nlm:2#t87 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; not copied |
| nlm-2-t89 | nlm:2#t90 → nlm:2#t89 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; not copied |
| nlm-2-t91 | nlm:2#t92 → nlm:2#t91 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; copied: 1 pasted, 1 lifted into archive notes |
| nlm-2-t93 | nlm:2#t94 → nlm:2#t93 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; not copied |
| nlm-2-t95 | nlm:2#t96 → nlm:2#t95 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; not copied |
| nlm-2-t97 | nlm:2#t98 → nlm:2#t97 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; not copied |
| nlm-2-t99 | nlm:2#t100 → nlm:2#t99 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; not copied |
| nlm-2-t101 | nlm:2#t102 → nlm:2#t101 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; not copied |
| nlm-2-t103 | nlm:2#t104 → nlm:2#t103 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; not copied |
| nlm-2-t105 | nlm:2#t106 → nlm:2#t105 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; not copied |
| nlm-2-t107 | nlm:2#t108 → nlm:2#t107 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; not copied |
| nlm-2-t109 | nlm:2#t110 → nlm:2#t109 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; not copied |
| nlm-2-t111 | nlm:2#t112 → nlm:2#t111 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; not copied |
| nlm-2-t113 | nlm:2#t114 → nlm:2#t113 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; not copied |
| nlm-2-t115 | nlm:2#t116 → nlm:2#t115 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; copied: 2 pasted, 0 lifted into archive notes |
| nlm-2-t117 | nlm:2#t118 → nlm:2#t117 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; copied: 1 pasted, 0 lifted into archive notes |
| nlm-2-t119 | nlm:2#t120 → nlm:2#t119 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; copied: 0 pasted, 3 lifted into archive notes |
| nlm-2-t121 | nlm:2#t122 → nlm:2#t121 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; not copied |
| nlm-2-t123 | nlm:2#t124 → nlm:2#t123 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; not copied |
| nlm-2-t125 | nlm:2#t126 → nlm:2#t125 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; copied: 0 pasted, 1 lifted into archive notes |
| nlm-2-t127 | nlm:2#t128 → nlm:2#t127 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; not copied |
| nlm-2-t129 | nlm:2#t130 → nlm:2#t129 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; not copied |
| nlm-2-t131 | nlm:2#t132 → nlm:2#t131 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; not copied |
| nlm-2-t133 | nlm:2#t134 → nlm:2#t133 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; not copied |
| nlm-2-t135 | nlm:2#t136 → nlm:2#t135 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; copied: 2 pasted, 0 lifted into archive notes |
| nlm-2-t137 | nlm:2#t138 → nlm:2#t137 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; copied: 1 pasted, 0 lifted into archive notes |
| nlm-2-t139 | nlm:2#t140 → nlm:2#t139 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; not copied |
| nlm-2-t141 | nlm:2#t142 → nlm:2#t141 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; not copied |
| nlm-2-t143 | nlm:2#t144 → nlm:2#t143 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; not copied |
| nlm-2-t145 | nlm:2#t146 → nlm:2#t145 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; not copied |
| nlm-2-t147 | nlm:2#t148 → nlm:2#t147 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; not copied |
| nlm-2-t149 | nlm:2#t150 → nlm:2#t149 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; not copied |
| nlm-2-t151 | nlm:2#t152 → nlm:2#t151 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; not copied |
| nlm-2-t153 | nlm:2#t154 → nlm:2#t153 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; not copied |
| nlm-2-t155 | nlm:2#t156 → nlm:2#t155 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; not copied |
| nlm-2-t157 | nlm:2#t158 → nlm:2#t157 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; not copied |
| nlm-2-t159 | nlm:2#t160 → nlm:2#t159 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; copied: 2 pasted, 0 lifted into archive notes |
| nlm-2-t161 | nlm:2#t162 → nlm:2#t161 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; not copied |
| nlm-2-t163 | nlm:2#t164 → nlm:2#t163 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; not copied |
| nlm-2-t165 | nlm:2#t166 → nlm:2#t165 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; not copied |
| nlm-2-t167 | nlm:2#t168 → nlm:2#t167 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; not copied |
| nlm-2-t169 | nlm:2#t170 → nlm:2#t169 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; not copied |
| nlm-2-t171 | nlm:2#t172 → nlm:2#t171 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; not copied |
| nlm-2-t173 | nlm:2#t174 → nlm:2#t173 | NotebookLM (lineage), notebook 2 "The Lioness of Tall Tale Story Plan", 2026-01-13; not copied |
| nlm-3-t1 | nlm:3#t2 → nlm:3#t1 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-3-t3 | nlm:3#t4 → nlm:3#t3 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-3-t5 | nlm:3#t6 → nlm:3#t5 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-3-t7 | nlm:3#t8 → nlm:3#t7 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-3-t9 | nlm:3#t10 → nlm:3#t9 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-3-t11 | nlm:3#t12 → nlm:3#t11 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-3-t13 | nlm:3#t14 → nlm:3#t13 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-3-t15 | nlm:3#t16 → nlm:3#t15 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-3-t17 | nlm:3#t18 → nlm:3#t17 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-3-t19 | nlm:3#t20 → nlm:3#t19 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-3-t21 | nlm:3#t22 → nlm:3#t21 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-3-t23 | nlm:3#t24 → nlm:3#t23 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-3-t25 | nlm:3#t26 → nlm:3#t25 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-3-t27 | nlm:3#t28 → nlm:3#t27 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-3-t29 | nlm:3#t30 → nlm:3#t29 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-3-t31 | nlm:3#t32 → nlm:3#t31 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-3-t33 | nlm:3#t34 → nlm:3#t33 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-3-t35 | nlm:3#t36 → nlm:3#t35 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-3-t37 | nlm:3#t38 → nlm:3#t37 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-3-t39 | nlm:3#t40 → nlm:3#t39 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-3-t41 | nlm:3#t42 → nlm:3#t41 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-3-t43 | nlm:3#t44 → nlm:3#t43 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-3-t45 | nlm:3#t46 → nlm:3#t45 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-3-t47 | nlm:3#t48 → nlm:3#t47 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-3-t49 | nlm:3#t50 → nlm:3#t49 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-3-t51 | nlm:3#t52 → nlm:3#t51 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-3-t53 | nlm:3#t54 → nlm:3#t53 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-3-t55 | nlm:3#t56 → nlm:3#t55 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-3-t57 | nlm:3#t58 → nlm:3#t57 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-3-t59 | nlm:3#t60 → nlm:3#t59 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-3-t61 | nlm:3#t62 → nlm:3#t61 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-3-t63 | nlm:3#t64 → nlm:3#t63 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-3-t65 | nlm:3#t66 → nlm:3#t65 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-3-t67 | nlm:3#t68 → nlm:3#t67 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-3-t69 | nlm:3#t70 → nlm:3#t69 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-3-t71 | nlm:3#t72 → nlm:3#t71 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-3-t73 | nlm:3#t74 → nlm:3#t73 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-3-t75 | nlm:3#t76 → nlm:3#t75 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-3-t77 | nlm:3#t78 → nlm:3#t77 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-3-t79 | nlm:3#t80 → nlm:3#t79 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-3-t81 | nlm:3#t82 → nlm:3#t81 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-3-t83 | nlm:3#t84 → nlm:3#t83 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-3-t85 | nlm:3#t86 → nlm:3#t85 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-3-t87 | nlm:3#t88 → nlm:3#t87 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-3-t89 | nlm:3#t90 → nlm:3#t89 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-3-t91 | nlm:3#t92 → nlm:3#t91 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-3-t93 | nlm:3#t94 → nlm:3#t93 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-3-t95 | nlm:3#t96 → nlm:3#t95 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-3-t97 | nlm:3#t98 → nlm:3#t97 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-3-t99 | nlm:3#t100 → nlm:3#t99 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-3-t101 | nlm:3#t102 → nlm:3#t101 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-3-t103 | nlm:3#t104 → nlm:3#t103 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-3-t105 | nlm:3#t106 → nlm:3#t105 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-3-t107 | nlm:3#t108 → nlm:3#t107 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-3-t109 | nlm:3#t110 → nlm:3#t109 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-3-t111 | nlm:3#t112 → nlm:3#t111 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-3-t113 | nlm:3#t114 → nlm:3#t113 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-3-t115 | nlm:3#t116 → nlm:3#t115 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-3-t117 | nlm:3#t118 → nlm:3#t117 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-3-t119 | nlm:3#t120 → nlm:3#t119 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-3-t121 | nlm:3#t122 → nlm:3#t121 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-3-t123 | nlm:3#t124 → nlm:3#t123 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-3-t125 | nlm:3#t126 → nlm:3#t125 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-3-t127 | nlm:3#t128 → nlm:3#t127 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-3-t129 | nlm:3#t130 → nlm:3#t129 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-3-t131 | nlm:3#t132 → nlm:3#t131 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-3-t133 | nlm:3#t134 → nlm:3#t133 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-3-t135 | nlm:3#t136 → nlm:3#t135 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-3-t137 | nlm:3#t138 → nlm:3#t137 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-3-t139 | nlm:3#t140 → nlm:3#t139 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-3-t141 | nlm:3#t142 → nlm:3#t141 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-3-t143 | nlm:3#t144 → nlm:3#t143 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-3-t145 | nlm:3#t146 → nlm:3#t145 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-3-t147 | nlm:3#t148 → nlm:3#t147 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-3-t149 | nlm:3#t150 → nlm:3#t149 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-3-t151 | nlm:3#t152 → nlm:3#t151 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-3-t153 | nlm:3#t154 → nlm:3#t153 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-3-t155 | nlm:3#t156 → nlm:3#t155 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-3-t157 | nlm:3#t158 → nlm:3#t157 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-3-t159 | nlm:3#t160 → nlm:3#t159 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-3-t161 | nlm:3#t162 → nlm:3#t161 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-3-t163 | nlm:3#t164 → nlm:3#t163 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-3-t165 | nlm:3#t166 → nlm:3#t165 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-3-t167 | nlm:3#t168 → nlm:3#t167 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-3-t169 | nlm:3#t170 → nlm:3#t169 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-3-t171 | nlm:3#t172 → nlm:3#t171 | NotebookLM (lineage), notebook 3 "Perspective Analysis", 2026-02-02; not copied |
| nlm-4-t1 | nlm:4#t2 → nlm:4#t1 | NotebookLM (lineage), notebook 4 "TLTT History", 2026-02-10; not copied |
| nlm-4-t3 | nlm:4#t4 → nlm:4#t3 | NotebookLM (lineage), notebook 4 "TLTT History", 2026-02-10; not copied |
| nlm-4-t5 | nlm:4#t6 → nlm:4#t5 | NotebookLM (lineage), notebook 4 "TLTT History", 2026-02-10; not copied |
| nlm-4-t7 | nlm:4#t8 → nlm:4#t7 | NotebookLM (lineage), notebook 4 "TLTT History", 2026-02-10; not copied |
| nlm-4-t9 | nlm:4#t10 → nlm:4#t9 | NotebookLM (lineage), notebook 4 "TLTT History", 2026-02-10; not copied |
| nlm-4-t11 | nlm:4#t12 → nlm:4#t11 | NotebookLM (lineage), notebook 4 "TLTT History", 2026-02-10; not copied |
| nlm-4-t13 | nlm:4#t14 → nlm:4#t13 | NotebookLM (lineage), notebook 4 "TLTT History", 2026-02-10; not copied |
| nlm-4-t15 | nlm:4#t16 → nlm:4#t15 | NotebookLM (lineage), notebook 4 "TLTT History", 2026-02-10; not copied |
| nlm-4-t17 | nlm:4#t18 → nlm:4#t17 | NotebookLM (lineage), notebook 4 "TLTT History", 2026-02-10; not copied |
| nlm-4-t19 | nlm:4#t20 → nlm:4#t19 | NotebookLM (lineage), notebook 4 "TLTT History", 2026-02-10; not copied |
| nlm-4-t21 | nlm:4#t22 → nlm:4#t21 | NotebookLM (lineage), notebook 4 "TLTT History", 2026-02-10; copied: 0 pasted, 1 lifted into archive notes |
| nlm-4-t23 | nlm:4#t24 → nlm:4#t23 | NotebookLM (lineage), notebook 4 "TLTT History", 2026-02-10; not copied |
| nlm-4-t25 | nlm:4#t26 → nlm:4#t25 | NotebookLM (lineage), notebook 4 "TLTT History", 2026-02-10; not copied |
| nlm-4-t27 | nlm:4#t28 → nlm:4#t27 | NotebookLM (lineage), notebook 4 "TLTT History", 2026-02-10; not copied |
| nlm-5-t1 | nlm:5#t2 → nlm:5#t1 | NotebookLM (lineage), notebook 5 "Ambition and Harmony: The Lioness of Tall Tale", 2026-02-11; not copied |
| nlm-5-t3 | nlm:5#t4 → nlm:5#t3 | NotebookLM (lineage), notebook 5 "Ambition and Harmony: The Lioness of Tall Tale", 2026-02-11; not copied |
| nlm-5-t5 | nlm:5#t6 → nlm:5#t5 | NotebookLM (lineage), notebook 5 "Ambition and Harmony: The Lioness of Tall Tale", 2026-02-11; copied: 0 pasted, 1 lifted into archive notes |
| nlm-6-t1 | nlm:6#t2 → nlm:6#t1 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t3 | nlm:6#t4 → nlm:6#t3 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t5 | nlm:6#t6 → nlm:6#t5 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t7 | nlm:6#t8 → nlm:6#t7 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t9 | nlm:6#t10 → nlm:6#t9 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t11 | nlm:6#t12 → nlm:6#t11 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t13 | nlm:6#t14 → nlm:6#t13 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t15 | nlm:6#t16 → nlm:6#t15 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t17 | nlm:6#t18 → nlm:6#t17 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t19 | nlm:6#t20 → nlm:6#t19 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t21 | nlm:6#t22 → nlm:6#t21 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t23 | nlm:6#t24 → nlm:6#t23 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t25 | nlm:6#t26 → nlm:6#t25 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t27 | nlm:6#t28 → nlm:6#t27 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t29 | nlm:6#t30 → nlm:6#t29 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t31 | nlm:6#t32 → nlm:6#t31 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t33 | nlm:6#t34 → nlm:6#t33 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t35 | nlm:6#t36 → nlm:6#t35 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t37 | nlm:6#t38 → nlm:6#t37 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t39 | nlm:6#t40 → nlm:6#t39 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t41 | nlm:6#t42 → nlm:6#t41 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; copied: 1 pasted, 0 lifted into archive notes |
| nlm-6-t43 | nlm:6#t44 → nlm:6#t43 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; copied: 1 pasted, 0 lifted into archive notes |
| nlm-6-t45 | nlm:6#t46 → nlm:6#t45 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t47 | nlm:6#t48 → nlm:6#t47 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t49 | nlm:6#t50 → nlm:6#t49 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t51 | nlm:6#t52 → nlm:6#t51 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t53 | nlm:6#t54 → nlm:6#t53 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t55 | nlm:6#t56 → nlm:6#t55 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t57 | nlm:6#t58 → nlm:6#t57 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t59 | nlm:6#t60 → nlm:6#t59 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t61 | nlm:6#t62 → nlm:6#t61 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t63 | nlm:6#t64 → nlm:6#t63 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; copied: 3 pasted, 0 lifted into archive notes |
| nlm-6-t65 | nlm:6#t66 → nlm:6#t65 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t67 | nlm:6#t68 → nlm:6#t67 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; copied: 3 pasted, 0 lifted into archive notes |
| nlm-6-t69 | nlm:6#t70 → nlm:6#t69 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t71 | nlm:6#t72 → nlm:6#t71 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; copied: 1 pasted, 0 lifted into archive notes |
| nlm-6-t73 | nlm:6#t74 → nlm:6#t73 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; copied: 1 pasted, 0 lifted into archive notes |
| nlm-6-t75 | nlm:6#t76 → nlm:6#t75 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; copied: 2 pasted, 1 lifted into archive notes |
| nlm-6-t77 | nlm:6#t78 → nlm:6#t77 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; copied: 2 pasted, 0 lifted into archive notes |
| nlm-6-t79 | nlm:6#t80 → nlm:6#t79 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; copied: 1 pasted, 0 lifted into archive notes |
| nlm-6-t81 | nlm:6#t82 → nlm:6#t81 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t83 | nlm:6#t84 → nlm:6#t83 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t85 | nlm:6#t86 → nlm:6#t85 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t87 | nlm:6#t88 → nlm:6#t87 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t89 | nlm:6#t90 → nlm:6#t89 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; copied: 1 pasted, 0 lifted into archive notes |
| nlm-6-t91 | nlm:6#t92 → nlm:6#t91 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; copied: 1 pasted, 0 lifted into archive notes |
| nlm-6-t93 | nlm:6#t94 → nlm:6#t93 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; copied: 2 pasted, 0 lifted into archive notes |
| nlm-6-t95 | nlm:6#t96 → nlm:6#t95 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; copied: 1 pasted, 0 lifted into archive notes |
| nlm-6-t97 | nlm:6#t98 → nlm:6#t97 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t99 | nlm:6#t100 → nlm:6#t99 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; copied: 1 pasted, 0 lifted into archive notes |
| nlm-6-t101 | nlm:6#t102 → nlm:6#t101 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; copied: 1 pasted, 0 lifted into archive notes |
| nlm-6-t103 | nlm:6#t104 → nlm:6#t103 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t105 | nlm:6#t106 → nlm:6#t105 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t107 | nlm:6#t108 → nlm:6#t107 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; copied: 1 pasted, 0 lifted into archive notes |
| nlm-6-t109 | nlm:6#t110 → nlm:6#t109 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; copied: 1 pasted, 0 lifted into archive notes |
| nlm-6-t111 | nlm:6#t112 → nlm:6#t111 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; copied: 1 pasted, 0 lifted into archive notes |
| nlm-6-t113 | nlm:6#t114 → nlm:6#t113 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; copied: 1 pasted, 0 lifted into archive notes |
| nlm-6-t115 | nlm:6#t116 → nlm:6#t115 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t117 | nlm:6#t118 → nlm:6#t117 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; copied: 2 pasted, 0 lifted into archive notes |
| nlm-6-t119 | nlm:6#t120 → nlm:6#t119 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t121 | nlm:6#t122 → nlm:6#t121 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; copied: 0 pasted, 1 lifted into archive notes |
| nlm-6-t123 | nlm:6#t124 → nlm:6#t123 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t125 | nlm:6#t126 → nlm:6#t125 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t127 | nlm:6#t128 → nlm:6#t127 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t129 | nlm:6#t130 → nlm:6#t129 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t131 | nlm:6#t132 → nlm:6#t131 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t133 | nlm:6#t134 → nlm:6#t133 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t135 | nlm:6#t136 → nlm:6#t135 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t137 | nlm:6#t138 → nlm:6#t137 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; copied: 1 pasted, 0 lifted into archive notes |
| nlm-6-t139 | nlm:6#t140 → nlm:6#t139 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t141 | nlm:6#t142 → nlm:6#t141 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; copied: 4 pasted, 0 lifted into archive notes |
| nlm-6-t143 | nlm:6#t144 → nlm:6#t143 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; copied: 1 pasted, 0 lifted into archive notes |
| nlm-6-t145 | nlm:6#t146 → nlm:6#t145 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t147 | nlm:6#t148 → nlm:6#t147 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t149 | nlm:6#t150 → nlm:6#t149 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t151 | nlm:6#t152 → nlm:6#t151 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; copied: 0 pasted, 1 lifted into archive notes |
| nlm-6-t153 | nlm:6#t154 → nlm:6#t153 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; copied: 0 pasted, 2 lifted into archive notes |
| nlm-6-t155 | nlm:6#t156 → nlm:6#t155 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t157 | nlm:6#t158 → nlm:6#t157 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; copied: 3 pasted, 0 lifted into archive notes |
| nlm-6-t159 | nlm:6#t160 → nlm:6#t159 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; copied: 1 pasted, 0 lifted into archive notes |
| nlm-6-t161 | nlm:6#t162 → nlm:6#t161 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; copied: 1 pasted, 0 lifted into archive notes |
| nlm-6-t163 | nlm:6#t164 → nlm:6#t163 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t165 | nlm:6#t166 → nlm:6#t165 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; copied: 1 pasted, 0 lifted into archive notes |
| nlm-6-t167 | nlm:6#t168 → nlm:6#t167 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; copied: 1 pasted, 0 lifted into archive notes |
| nlm-6-t169 | nlm:6#t170 → nlm:6#t169 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t171 | nlm:6#t172 → nlm:6#t171 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; copied: 0 pasted, 1 lifted into archive notes |
| nlm-6-t173 | nlm:6#t174 → nlm:6#t173 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; copied: 1 pasted, 0 lifted into archive notes |
| nlm-6-t175 | nlm:6#t176 → nlm:6#t175 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t177 | nlm:6#t178 → nlm:6#t177 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t179 | nlm:6#t180 → nlm:6#t179 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; copied: 1 pasted, 0 lifted into archive notes |
| nlm-6-t181 | nlm:6#t182 → nlm:6#t181 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t183 | nlm:6#t184 → nlm:6#t183 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t185 | nlm:6#t186 → nlm:6#t185 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; copied: 1 pasted, 1 lifted into archive notes |
| nlm-6-t187 | nlm:6#t188 → nlm:6#t187 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; copied: 2 pasted, 0 lifted into archive notes |
| nlm-6-t189 | nlm:6#t190 → nlm:6#t189 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; copied: 3 pasted, 0 lifted into archive notes |
| nlm-6-t191 | nlm:6#t192 → nlm:6#t191 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t193 | nlm:6#t194 → nlm:6#t193 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; copied: 1 pasted, 0 lifted into archive notes |
| nlm-6-t195 | nlm:6#t196 → nlm:6#t195 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t197 | nlm:6#t198 → nlm:6#t197 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t199 | nlm:6#t200 → nlm:6#t199 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; copied: 1 pasted, 1 lifted into archive notes |
| nlm-6-t201 | nlm:6#t202 → nlm:6#t201 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t203 | nlm:6#t204 → nlm:6#t203 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; copied: 0 pasted, 1 lifted into archive notes |
| nlm-6-t205 | nlm:6#t206 → nlm:6#t205 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t207 | nlm:6#t208 → nlm:6#t207 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; copied: 1 pasted, 0 lifted into archive notes |
| nlm-6-t209 | nlm:6#t210 → nlm:6#t209 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t211 | nlm:6#t212 → nlm:6#t211 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t213 | nlm:6#t214 → nlm:6#t213 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t215 | nlm:6#t216 → nlm:6#t215 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t217 | nlm:6#t218 → nlm:6#t217 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t219 | nlm:6#t220 → nlm:6#t219 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t221 | nlm:6#t222 → nlm:6#t221 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t223 | nlm:6#t224 → nlm:6#t223 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t225 | nlm:6#t226 → nlm:6#t225 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t227 | nlm:6#t228 → nlm:6#t227 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t229 | nlm:6#t230 → nlm:6#t229 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t231 | nlm:6#t232 → nlm:6#t231 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t233 | nlm:6#t234 → nlm:6#t233 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; copied: 1 pasted, 0 lifted into archive notes |
| nlm-6-t235 | nlm:6#t236 → nlm:6#t235 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t237 | nlm:6#t238 → nlm:6#t237 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t239 | nlm:6#t240 → nlm:6#t239 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t241 | nlm:6#t242 → nlm:6#t241 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t243 | nlm:6#t244 → nlm:6#t243 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t245 | nlm:6#t246 → nlm:6#t245 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t247 | nlm:6#t248 → nlm:6#t247 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t249 | nlm:6#t250 → nlm:6#t249 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; copied: 0 pasted, 1 lifted into archive notes |
| nlm-6-t251 | nlm:6#t252 → nlm:6#t251 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t253 | nlm:6#t254 → nlm:6#t253 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t255 | nlm:6#t256 → nlm:6#t255 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t257 | nlm:6#t258 → nlm:6#t257 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t259 | nlm:6#t260 → nlm:6#t259 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t261 | nlm:6#t262 → nlm:6#t261 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t263 | nlm:6#t264 → nlm:6#t263 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; copied: 0 pasted, 1 lifted into archive notes |
| nlm-6-t265 | nlm:6#t266 → nlm:6#t265 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t267 | nlm:6#t268 → nlm:6#t267 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t269 | nlm:6#t270 → nlm:6#t269 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; copied: 1 pasted, 0 lifted into archive notes |
| nlm-6-t271 | nlm:6#t272 → nlm:6#t271 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t273 | nlm:6#t274 → nlm:6#t273 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t275 | nlm:6#t276 → nlm:6#t275 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t277 | nlm:6#t278 → nlm:6#t277 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; copied: 1 pasted, 0 lifted into archive notes |
| nlm-6-t279 | nlm:6#t280 → nlm:6#t279 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t281 | nlm:6#t282 → nlm:6#t281 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t283 | nlm:6#t284 → nlm:6#t283 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t285 | nlm:6#t286 → nlm:6#t285 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t287 | nlm:6#t288 → nlm:6#t287 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t289 | nlm:6#t290 → nlm:6#t289 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; copied: 0 pasted, 1 lifted into archive notes |
| nlm-6-t291 | nlm:6#t292 → nlm:6#t291 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t293 | nlm:6#t294 → nlm:6#t293 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t295 | nlm:6#t296 → nlm:6#t295 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t297 | nlm:6#t298 → nlm:6#t297 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t299 | nlm:6#t300 → nlm:6#t299 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t301 | nlm:6#t302 → nlm:6#t301 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t303 | nlm:6#t304 → nlm:6#t303 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; copied: 1 pasted, 0 lifted into archive notes |
| nlm-6-t305 | nlm:6#t306 → nlm:6#t305 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; copied: 1 pasted, 1 lifted into archive notes |
| nlm-6-t307 | nlm:6#t308 → nlm:6#t307 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t309 | nlm:6#t310 → nlm:6#t309 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t311 | nlm:6#t312 → nlm:6#t311 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t313 | nlm:6#t314 → nlm:6#t313 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t315 | nlm:6#t316 → nlm:6#t315 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; copied: 0 pasted, 1 lifted into archive notes |
| nlm-6-t317 | nlm:6#t318 → nlm:6#t317 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t319 | nlm:6#t320 → nlm:6#t319 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t321 | nlm:6#t322 → nlm:6#t321 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t323 | nlm:6#t324 → nlm:6#t323 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; copied: 0 pasted, 1 lifted into archive notes |
| nlm-6-t325 | nlm:6#t326 → nlm:6#t325 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t327 | nlm:6#t328 → nlm:6#t327 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t329 | nlm:6#t330 → nlm:6#t329 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t331 | nlm:6#t332 → nlm:6#t331 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; copied: 0 pasted, 1 lifted into archive notes |
| nlm-6-t333 | nlm:6#t334 → nlm:6#t333 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t335 | nlm:6#t336 → nlm:6#t335 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; copied: 0 pasted, 1 lifted into archive notes |
| nlm-6-t337 | nlm:6#t338 → nlm:6#t337 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; copied: 1 pasted, 0 lifted into archive notes |
| nlm-6-t339 | nlm:6#t340 → nlm:6#t339 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t341 | nlm:6#t342 → nlm:6#t341 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t343 | nlm:6#t344 → nlm:6#t343 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t345 | nlm:6#t346 → nlm:6#t345 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t347 | nlm:6#t348 → nlm:6#t347 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t349 | nlm:6#t350 → nlm:6#t349 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t351 | nlm:6#t352 → nlm:6#t351 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t353 | nlm:6#t354 → nlm:6#t353 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t355 | nlm:6#t356 → nlm:6#t355 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; copied: 1 pasted, 0 lifted into archive notes |
| nlm-6-t357 | nlm:6#t358 → nlm:6#t357 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t359 | nlm:6#t360 → nlm:6#t359 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; copied: 1 pasted, 0 lifted into archive notes |
| nlm-6-t361 | nlm:6#t362 → nlm:6#t361 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-6-t363 | nlm:6#t364 → nlm:6#t363 | NotebookLM (lineage), notebook 6 "Refinement of Aquileian Lore", 2026-02-13; not copied |
| nlm-7-t1 | nlm:7#t2 → nlm:7#t1 | NotebookLM (lineage), notebook 7 "Prompt History 12-27-25 to 3-23-26 Try 2", 2026-03-23; not copied |
| nlm-7-t3 | nlm:7#t4 → nlm:7#t3 | NotebookLM (lineage), notebook 7 "Prompt History 12-27-25 to 3-23-26 Try 2", 2026-03-23; not copied |
| nlm-7-t5 | nlm:7#t6 → nlm:7#t5 | NotebookLM (lineage), notebook 7 "Prompt History 12-27-25 to 3-23-26 Try 2", 2026-03-23; not copied |
| nlm-7-t7 | nlm:7#t8 → nlm:7#t7 | NotebookLM (lineage), notebook 7 "Prompt History 12-27-25 to 3-23-26 Try 2", 2026-03-23; not copied |
| nlm-7-t9 | nlm:7#t10 → nlm:7#t9 | NotebookLM (lineage), notebook 7 "Prompt History 12-27-25 to 3-23-26 Try 2", 2026-03-23; not copied |
| nlm-7-t11 | nlm:7#t12 → nlm:7#t11 | NotebookLM (lineage), notebook 7 "Prompt History 12-27-25 to 3-23-26 Try 2", 2026-03-23; not copied |
| nlm-7-t13 | nlm:7#t14 → nlm:7#t13 | NotebookLM (lineage), notebook 7 "Prompt History 12-27-25 to 3-23-26 Try 2", 2026-03-23; not copied |
| nlm-7-t15 | nlm:7#t16 → nlm:7#t15 | NotebookLM (lineage), notebook 7 "Prompt History 12-27-25 to 3-23-26 Try 2", 2026-03-23; not copied |
| nlm-7-t17 | nlm:7#t18 → nlm:7#t17 | NotebookLM (lineage), notebook 7 "Prompt History 12-27-25 to 3-23-26 Try 2", 2026-03-23; not copied |
| nlm-7-t19 | nlm:7#t20 → nlm:7#t19 | NotebookLM (lineage), notebook 7 "Prompt History 12-27-25 to 3-23-26 Try 2", 2026-03-23; not copied |
| nlm-7-t21 | nlm:7#t22 → nlm:7#t21 | NotebookLM (lineage), notebook 7 "Prompt History 12-27-25 to 3-23-26 Try 2", 2026-03-23; not copied |
| nlm-7-t23 | nlm:7#t24 → nlm:7#t23 | NotebookLM (lineage), notebook 7 "Prompt History 12-27-25 to 3-23-26 Try 2", 2026-03-23; not copied |
| nlm-7-t25 | nlm:7#t26 → nlm:7#t25 | NotebookLM (lineage), notebook 7 "Prompt History 12-27-25 to 3-23-26 Try 2", 2026-03-23; not copied |
| nlm-7-t27 | nlm:7#t28 → nlm:7#t27 | NotebookLM (lineage), notebook 7 "Prompt History 12-27-25 to 3-23-26 Try 2", 2026-03-23; not copied |
| nlm-7-t29 | nlm:7#t30 → nlm:7#t29 | NotebookLM (lineage), notebook 7 "Prompt History 12-27-25 to 3-23-26 Try 2", 2026-03-23; not copied |
| nlm-7-t31 | nlm:7#t32 → nlm:7#t31 | NotebookLM (lineage), notebook 7 "Prompt History 12-27-25 to 3-23-26 Try 2", 2026-03-23; not copied |
| nlm-7-t33 | nlm:7#t34 → nlm:7#t33 | NotebookLM (lineage), notebook 7 "Prompt History 12-27-25 to 3-23-26 Try 2", 2026-03-23; not copied |
| nlm-7-t35 | nlm:7#t36 → nlm:7#t35 | NotebookLM (lineage), notebook 7 "Prompt History 12-27-25 to 3-23-26 Try 2", 2026-03-23; not copied |
| nlm-7-t37 | nlm:7#t38 → nlm:7#t37 | NotebookLM (lineage), notebook 7 "Prompt History 12-27-25 to 3-23-26 Try 2", 2026-03-23; not copied |
| nlm-7-t39 | nlm:7#t40 → nlm:7#t39 | NotebookLM (lineage), notebook 7 "Prompt History 12-27-25 to 3-23-26 Try 2", 2026-03-23; not copied |
| nlm-7-t41 | nlm:7#t42 → nlm:7#t41 | NotebookLM (lineage), notebook 7 "Prompt History 12-27-25 to 3-23-26 Try 2", 2026-03-23; not copied |
| nlm-7-t43 | nlm:7#t44 → nlm:7#t43 | NotebookLM (lineage), notebook 7 "Prompt History 12-27-25 to 3-23-26 Try 2", 2026-03-23; not copied |
| nlm-8-t1 | nlm:8#t2 → nlm:8#t1 | NotebookLM (lineage), notebook 8 "MLP Transcripts and TLTT Story Plan", 2026-03-30; not copied |
| nlm-8-t3 | nlm:8#t4 → nlm:8#t3 | NotebookLM (lineage), notebook 8 "MLP Transcripts and TLTT Story Plan", 2026-03-30; not copied |
| nlm-8-t5 | nlm:8#t6 → nlm:8#t5 | NotebookLM (lineage), notebook 8 "MLP Transcripts and TLTT Story Plan", 2026-03-30; copied: 1 pasted, 0 lifted into archive notes |
| nlm-8-t7 | nlm:8#t8 → nlm:8#t7 | NotebookLM (lineage), notebook 8 "MLP Transcripts and TLTT Story Plan", 2026-03-30; not copied |
| nlm-8-t9 | nlm:8#t10 → nlm:8#t9 | NotebookLM (lineage), notebook 8 "MLP Transcripts and TLTT Story Plan", 2026-03-30; not copied |
| nlm-8-t11 | nlm:8#t12 → nlm:8#t11 | NotebookLM (lineage), notebook 8 "MLP Transcripts and TLTT Story Plan", 2026-03-30; copied: 1 pasted, 0 lifted into archive notes |
| nlm-8-t13 | nlm:8#t14 → nlm:8#t13 | NotebookLM (lineage), notebook 8 "MLP Transcripts and TLTT Story Plan", 2026-03-30; not copied |
| nlm-8-t15 | nlm:8#t16 → nlm:8#t15 | NotebookLM (lineage), notebook 8 "MLP Transcripts and TLTT Story Plan", 2026-03-30; not copied |
| nlm-8-t17 | nlm:8#t18 → nlm:8#t17 | NotebookLM (lineage), notebook 8 "MLP Transcripts and TLTT Story Plan", 2026-03-30; not copied |
