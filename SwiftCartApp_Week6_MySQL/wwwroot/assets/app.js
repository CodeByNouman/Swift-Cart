// Week 6 (Sprint 1 + 2): navbar with the signed-in name, sign up / sign in, products loaded from SQL.
const $ = (s, r = document) => r.querySelector(s);
const fmt = n => 'Rs. ' + n.toLocaleString('en-PK');
const get = (k, d) => { try { return JSON.parse(localStorage.getItem(k)) ?? d; } catch { return d; } };
const set = (k, v) => localStorage.setItem(k, JSON.stringify(v));
const esc = s => String(s).replace(/[&<>"']/g, c => ({ '&': '&amp;', '<': '&lt;', '>': '&gt;', '"': '&quot;', "'": '&#39;' }[c]));
const fdate = (d, o) => new Date(d).toLocaleDateString('en-GB', o || { day: 'numeric', month: 'short', year: 'numeric' });

async function api(url, opts = {}) {
  const res = await fetch(url, {
    method: opts.method || 'GET',
    headers: { 'Content-Type': 'application/json' },
    credentials: 'same-origin',
    body: opts.body ? JSON.stringify(opts.body) : undefined
  });
  let data = null;
  try { data = await res.json(); } catch { }
  let message = data && data.message ? data.message : '';
  if (!message && data && data.errors) message = Object.values(data.errors)[0][0];
  return { ok: res.ok, status: res.status, data, message };
}

// ---- products (Sprint 2) ----
let PRODUCTS = [];
async function loadProducts() {
  const r = await api('/api/products');
  PRODUCTS = r.ok ? r.data.map(x => ({
    id: x.id, n: x.name, c: x.category, p: x.price, s: x.stock, e: x.emoji, bg: x.bgColor,
    hasExp: x.hasExpiry, exp: x.expiryDate
  })) : [];
  return r.ok;
}

function card(p) {
  const stock = p.s < 1 ? '<span class="badge text-bg-danger">Out of stock</span>'
    : p.s < 5 ? '<span class="badge text-bg-warning">Only ' + p.s + ' left</span>'
    : '<span class="badge text-bg-success-subtle text-success">In stock</span>';
  const exp = p.hasExp
    ? '<small class="text-secondary"><i class="bi bi-hourglass-split"></i> Expires ' + fdate(p.exp, { month: 'short', year: 'numeric' }) + '</small>'
    : '';
  return `<div class="col-6 col-md-4 col-lg-3">
    <div class="pcard">
      <div class="pimg" style="background:${esc(p.bg)}">${p.e}</div>
      <div class="p-3 d-flex flex-column gap-1 flex-grow-1">
        <small class="text-secondary">${esc(p.c)}</small>
        <span class="fw-semibold">${esc(p.n)}</span>
        ${exp}
        ${stock}
        <div class="price mt-auto pt-2">${fmt(p.p)}</div>
      </div>
    </div>
  </div>`;
}

// ---- layout + login (Sprint 1) ----
function toast(msg, type = 'success') {
  let w = $('#toasts');
  if (!w) {
    w = document.createElement('div');
    w.id = 'toasts';
    w.className = 'toast-container position-fixed top-0 end-0 p-3';
    document.body.append(w);
  }
  const t = document.createElement('div');
  t.className = 'toast align-items-center text-bg-' + type + ' border-0';
  t.innerHTML = '<div class="d-flex"><div class="toast-body fw-semibold">' + esc(msg) + '</div>'
    + '<button class="btn-close btn-close-white me-2 m-auto" data-bs-dismiss="toast"></button></div>';
  w.append(t);
  new bootstrap.Toast(t, { delay: 2500 }).show();
  t.addEventListener('hidden.bs.toast', () => t.remove());
}

async function logout() {
  await api('/api/auth/logout', { method: 'POST' });
  localStorage.removeItem('sc_seller');
  location = 'index.html';
}

function layout() {
  const s = get('sc_seller', null);
  $('#nav').innerHTML = `<nav class="navbar sticky-top">
    <div class="container">
      <a class="brand" href="index.html"><i class="bi bi-lightning-charge-fill"></i> SwiftCart</a>
      <div class="d-flex gap-2 align-items-center">
        ${s
          ? `<span class="fw-semibold"><i class="bi bi-shop"></i> Hi, ${esc(s.store)}</span>
             <button class="btn btn-outline-dark btn-sm" onclick="logout()">Sign out</button>`
          : `<a href="signin.html" class="btn btn-link text-dark">Seller sign in</a>
             <a href="signup.html" class="btn btn-primary">Sell on SwiftCart</a>`}
      </div>
    </div>
  </nav>`;
  const f = $('#foot');
  if (f) f.innerHTML = '<footer class="container py-5 mt-5 border-top small d-flex flex-wrap justify-content-between gap-2">'
    + '<span><b class="brand">SwiftCart</b> · Swift, simple shopping for Pakistan</span>'
    + '<span>Software Engineering project · UET Lahore</span></footer>';
}

// Bootstrap validation: runs the browser's checks, then fires a "valid" event on the form.
function check(form, extra) {
  form.addEventListener('submit', e => {
    e.preventDefault();
    e.stopPropagation();
    let ok = form.checkValidity();
    if (extra && !extra(form)) ok = false;
    form.classList.add('was-validated');
    if (ok) form.dispatchEvent(new CustomEvent('valid'));
  });
}

document.addEventListener('DOMContentLoaded', layout);
