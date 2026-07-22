const getErrorMessage = problem => {
  if (problem?.errors) {
    const messages = Object.values(problem.errors).flat();
    if (messages.length > 0) {
      return messages.join(' ');
    }
  }

  return problem?.detail || problem?.title || '計算失敗，請稍後再試。';
};

export const postCalculation = async (metric, payload) => {
  const response = await fetch(`/api/calculate/${metric}`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify(payload),
  });
  const data = await response.json().catch(() => null);

  if (!response.ok) {
    throw new Error(getErrorMessage(data));
  }

  return data;
};