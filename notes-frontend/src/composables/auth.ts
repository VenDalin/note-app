export interface DecodedJwt {
  userId?: number;
  username?: string;
  exp: number;
}

export function decodeJwt(token: string): DecodedJwt | null {
  try {
    if (!token) return null;
    const base64Url = token.split('.')[1];
    const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    const jsonPayload = decodeURIComponent(
      atob(base64)
        .split('')
        .map((c) => '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2))
        .join('')
    );
    const payload = JSON.parse(jsonPayload);
    return {
      userId: payload.id ,
      username: payload.username ,
      exp: payload.exp,
    };
  } catch (error) {
    console.error('Error decoding JWT:', error);
    return null;
  }
}

export function isAuthenticated(): boolean {
  const token = localStorage.getItem('token');
  if (token) {
    const decodedToken = decodeJwt(token);
    if (decodedToken && decodedToken.exp) {
      const isTokenExpired = decodedToken.exp < Date.now() / 1000;
      return !isTokenExpired;
    }
  }
  return false;
}