export async function getCartCount() {
  const res = await fetch("/api/cart/count", {
    credentials: "include",
    cache: "no-store"
  });

  if (!res.ok) throw new Error("Failed");
  return await res.json();
}

export async function addToCart(productId, quantity = 1) {
  const res = await fetch(
    `/api/cart/add?productId=${productId}&quantity=${quantity}`,
    {
      method: "POST",
      credentials: "include"
    }
  );

  return await res.json();
}