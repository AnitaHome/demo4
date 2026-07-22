export const GENDERS = ['male', 'female'];
export const ACTIVITY_LEVELS = ['sedentary', 'light', 'moderate', 'heavy', 'athlete'];

export const parsePositiveNumber = value => {
  if (typeof value !== 'string' || value.trim() === '') {
    return null;
  }

  const number = Number(value);
  return Number.isFinite(number) && number > 0 ? number : null;
};

export const parsePositiveInteger = value => {
  const number = parsePositiveNumber(value);
  return number !== null && Number.isInteger(number) ? number : null;
};

export const isBmiValid = fields => (
  parsePositiveNumber(fields.height) !== null
  && parsePositiveNumber(fields.weight) !== null
);

export const isBmrValid = fields => (
  isBmiValid(fields)
  && parsePositiveInteger(fields.age) !== null
  && GENDERS.includes(fields.gender)
);

export const isTdeeValid = fields => (
  isBmrValid(fields)
  && ACTIVITY_LEVELS.includes(fields.activityLevel)
);