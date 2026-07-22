import { postCalculation } from './api-client.js';
import {
  isBmiValid,
  isBmrValid,
  isTdeeValid,
  parsePositiveInteger,
  parsePositiveNumber,
} from './validation.js';

const form = document.querySelector('#calculator-form');
const heightInput = document.querySelector('#height');
const weightInput = document.querySelector('#weight');
const ageInput = document.querySelector('#age');
const activityInput = document.querySelector('#activity-level');
const bmiButton = document.querySelector('#btn-bmi');
const bmrButton = document.querySelector('#btn-bmr');
const tdeeButton = document.querySelector('#btn-tdee');
const resultRegion = document.querySelector('#result');
const errorRegion = document.querySelector('#api-error');

const getFields = () => ({
  height: heightInput.value,
  weight: weightInput.value,
  age: ageInput.value,
  gender: form.elements.gender.value,
  activityLevel: activityInput.value,
});

const showFieldError = (input, parser, message) => {
  const error = document.querySelector(`#${input.id}-error`);
  const isInvalid = input.value !== '' && parser(input.value) === null;
  input.setAttribute('aria-invalid', String(isInvalid));
  error.textContent = isInvalid ? message : '';
};

const updateState = () => {
  const fields = getFields();
  bmiButton.disabled = !isBmiValid(fields);
  bmrButton.disabled = !isBmrValid(fields);
  tdeeButton.disabled = !isTdeeValid(fields);

  showFieldError(heightInput, parsePositiveNumber, '請輸入大於 0 的身高。');
  showFieldError(weightInput, parsePositiveNumber, '請輸入大於 0 的體重。');
  showFieldError(ageInput, parsePositiveInteger, '請輸入大於 0 的整數年齡。');
};

const getPayload = metric => {
  const fields = getFields();
  const payload = {
    height: parsePositiveNumber(fields.height),
    weight: parsePositiveNumber(fields.weight),
  };

  if (metric !== 'bmi') {
    payload.age = parsePositiveInteger(fields.age);
    payload.gender = fields.gender;
  }

  if (metric === 'tdee') {
    payload.activityLevel = fields.activityLevel;
  }

  return payload;
};

const renderResult = (metric, data) => {
  const templates = {
    bmi: () => `<p class="text-sm text-slate-300">BMI</p><p class="mt-2 font-display text-5xl font-bold text-white">${data.bmi.toFixed(2)}</p><p class="mt-4 inline-block border border-emerald-400 px-3 py-1 text-sm text-emerald-200">${data.category}</p>`,
    bmr: () => `<p class="text-sm text-slate-300">基礎代謝率</p><p class="mt-2 font-display text-5xl font-bold text-white">${data.bmr.toFixed(2)}</p><p class="mt-4 text-sm text-slate-300">kcal／日</p>`,
    tdee: () => `<p class="text-sm text-slate-300">每日總熱量消耗</p><p class="mt-2 font-display text-5xl font-bold text-white">${data.tdee.toFixed(2)}</p><p class="mt-4 text-sm text-slate-300">kcal／日 · BMR ${data.bmr.toFixed(2)}</p>`,
  };
  resultRegion.innerHTML = templates[metric]();
};

const calculate = async (metric, button) => {
  const originalText = button.textContent;
  errorRegion.textContent = '';
  button.disabled = true;
  button.setAttribute('aria-busy', 'true');
  button.textContent = '計算中…';

  try {
    const data = await postCalculation(metric, getPayload(metric));
    renderResult(metric, data);
  } catch (error) {
    errorRegion.textContent = error instanceof Error ? error.message : '計算失敗，請稍後再試。';
  } finally {
    button.removeAttribute('aria-busy');
    button.textContent = originalText;
    updateState();
  }
};

form.addEventListener('input', updateState);
form.addEventListener('change', updateState);
bmiButton.addEventListener('click', () => calculate('bmi', bmiButton));
bmrButton.addEventListener('click', () => calculate('bmr', bmrButton));
tdeeButton.addEventListener('click', () => calculate('tdee', tdeeButton));

updateState();