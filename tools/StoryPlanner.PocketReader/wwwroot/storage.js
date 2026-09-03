// IndexedDB for the picked .storyplan bytes, localStorage for small preferences.
// Nothing here ever leaves the device. Imported by Services/Interop.cs.

const DB_NAME = "pocket-reader";
const STORE = "plans";
const staged = {};

function openDb() {
    return new Promise((resolve, reject) => {
        const req = indexedDB.open(DB_NAME, 1);
        req.onupgradeneeded = () => req.result.createObjectStore(STORE);
        req.onsuccess = () => resolve(req.result);
        req.onerror = () => reject(req.error);
    });
}

function tx(db, mode, fn) {
    return new Promise((resolve, reject) => {
        const t = db.transaction(STORE, mode);
        const req = fn(t.objectStore(STORE));
        req.onsuccess = () => resolve(req.result);
        req.onerror = () => reject(req.error);
    });
}

export async function prepare(slug) {
    try {
        const db = await openDb();
        const rec = await tx(db, "readonly", s => s.get(slug));
        db.close();
        if (!rec) return null;
        staged[slug] = rec.bytes;
        return JSON.stringify({ name: rec.name, pickedAt: rec.pickedAt });
    } catch (e) {
        console.warn("pocket-reader: could not read stored plan", e);
        return null;
    }
}

export function take(slug) {
    const bytes = staged[slug];
    delete staged[slug];
    return bytes ? new Uint8Array(bytes) : null;
}

export async function save(slug, name, pickedAt, bytes) {
    const db = await openDb();
    // Copy: the marshalled view may be backed by WASM memory that is released after the call.
    const copy = new Uint8Array(bytes).slice();
    await tx(db, "readwrite", s => s.put({ name, pickedAt, bytes: copy }, slug));
    db.close();
}

export async function remove(slug) {
    const db = await openDb();
    await tx(db, "readwrite", s => s.delete(slug));
    db.close();
}

export function getPref(key) {
    try { return localStorage.getItem(key); } catch { return null; }
}

export function setPref(key, value) {
    try { localStorage.setItem(key, value); } catch { /* private mode etc. */ }
}
