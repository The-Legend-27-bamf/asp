(function () {
	function animateCards(container) {
		if (!container) {
			return;
		}

		const cards = container.querySelectorAll('.entity-card, .grade-item');
		cards.forEach(function (card, index) {
			card.classList.remove('card-enter');
			card.classList.remove('card-pop');
			card.style.setProperty('--stagger-index', String(index));
			void card.offsetWidth;
			card.classList.add('card-enter');
			if (index < 2) {
				window.setTimeout(function () {
					card.classList.add('card-pop');
					window.setTimeout(function () {
						card.classList.remove('card-pop');
					}, 320);
				}, 90 + (index * 55));
			}
		});
	}

	function debounce(fn, wait) {
		let timer = null;
		return function (...args) {
			window.clearTimeout(timer);
			timer = window.setTimeout(function () {
				fn.apply(null, args);
			}, wait);
		};
	}

	function initAjaxListSearch() {
		const roots = document.querySelectorAll('[data-ajax-list-root]');
		roots.forEach(function (root) {
			const searchUrl = root.dataset.searchUrl;
			const targetId = root.dataset.targetId;
			const input = root.querySelector('[data-ajax-search-input]');
			const target = targetId ? document.getElementById(targetId) : null;
			if (!searchUrl || !input || !target) {
				return;
			}

			let controller = null;
			const runSearch = debounce(function () {
				const q = input.value || '';
				const url = new URL(searchUrl, window.location.origin);
				url.searchParams.set('q', q);

				if (controller) {
					controller.abort();
				}
				controller = new AbortController();
				target.classList.add('is-loading');
				root.classList.add('is-searching');

				fetch(url.toString(), {
					headers: {
						'X-Requested-With': 'XMLHttpRequest'
					},
					signal: controller.signal
				})
					.then(function (response) {
						if (!response.ok) {
							throw new Error('Search request failed.');
						}
						return response.text();
					})
					.then(function (html) {
						target.classList.add('is-refreshing');
						target.innerHTML = html;
						animateCards(target);
						target.classList.remove('is-refreshing');
						target.classList.add('is-fresh');
						window.setTimeout(function () {
							target.classList.remove('is-fresh');
						}, 280);
					})
					.catch(function (error) {
						if (error && error.name === 'AbortError') {
							return;
						}
						target.innerHTML = '<p class="empty-note">Dogodila se greska pri pretrazi.</p>';
					})
					.finally(function () {
						target.classList.remove('is-loading');
						root.classList.remove('is-searching');
					});
			}, 280);

			input.addEventListener('input', runSearch);
		});
	}

	function initAutocompleteDropdowns() {
		const roots = document.querySelectorAll('[data-autocomplete-root]');
		roots.forEach(function (root) {
			const endpoint = root.dataset.endpoint;
			const minChars = Number(root.dataset.minChars || 2);
			const input = root.querySelector('[data-autocomplete-input]');
			const menu = root.querySelector('[data-autocomplete-menu]');
			const hiddenInput = root.querySelector('[data-autocomplete-hidden]');
			if (!endpoint || !input || !menu) {
				return;
			}

			let selectedIndex = -1;
			let items = [];
			let controller = null;

			function clearMenu() {
				menu.innerHTML = '';
				menu.classList.remove('is-open');
				menu.hidden = true;
				selectedIndex = -1;
			}

			function selectItem(item) {
				input.value = item.label || '';
				if (hiddenInput) {
					hiddenInput.value = item.value != null ? String(item.value) : '';
				}
				root.classList.remove('is-selected');
				void root.offsetWidth;
				root.classList.add('is-selected');
				window.setTimeout(function () {
					root.classList.remove('is-selected');
				}, 380);
				clearMenu();

				root.dispatchEvent(new CustomEvent('autocomplete:selected', {
					bubbles: true,
					detail: item
				}));
			}

			function renderMenu() {
				if (!items.length) {
					clearMenu();
					return;
				}

				menu.innerHTML = items.map(function (item, index) {
					const meta = item.meta ? '<span class="autocomplete-item-meta">' + item.meta + '</span>' : '';
					return '<button type="button" class="autocomplete-item' +
						(index === selectedIndex ? ' is-active' : '') +
						'" data-index="' + index + '"><span>' + item.label + '</span>' + meta + '</button>';
				}).join('');
				menu.hidden = false;

				menu.querySelectorAll('.autocomplete-item').forEach(function (button) {
					button.addEventListener('mousedown', function (event) {
						event.preventDefault();
					});
					button.addEventListener('click', function () {
						const index = Number(button.dataset.index);
						const item = items[index];
						if (item) {
							selectItem(item);
						}
					});
				});

				menu.classList.add('is-open');
			}

			const runQuery = debounce(function () {
				const query = input.value.trim();
				if (query.length < minChars) {
					clearMenu();
					return;
				}

				const url = new URL(endpoint, window.location.origin);
				url.searchParams.set('q', query);

				if (controller) {
					controller.abort();
				}
				controller = new AbortController();

				fetch(url.toString(), {
					headers: {
						'X-Requested-With': 'XMLHttpRequest'
					},
					signal: controller.signal
				})
					.then(function (response) {
						if (!response.ok) {
							throw new Error('Autocomplete request failed.');
						}
						return response.json();
					})
					.then(function (data) {
						items = Array.isArray(data) ? data : [];
						selectedIndex = -1;
						renderMenu();
					})
					.catch(function (error) {
						if (error && error.name === 'AbortError') {
							return;
						}
						clearMenu();
					});
			}, 220);

			input.addEventListener('input', function () {
				runQuery();
			});

			input.addEventListener('keydown', function (event) {
				if (menu.hidden || !items.length) {
					return;
				}

				if (event.key === 'ArrowDown') {
					event.preventDefault();
					selectedIndex = Math.min(selectedIndex + 1, items.length - 1);
					renderMenu();
				}

				if (event.key === 'ArrowUp') {
					event.preventDefault();
					selectedIndex = Math.max(selectedIndex - 1, 0);
					renderMenu();
				}

				if (event.key === 'Enter') {
					event.preventDefault();
					if (selectedIndex >= 0 && items[selectedIndex]) {
						selectItem(items[selectedIndex]);
					}
				}

				if (event.key === 'Escape') {
					clearMenu();
				}
			});

			input.addEventListener('blur', function () {
				window.setTimeout(clearMenu, 120);
			});
		});
	}

	function initFormMicroFeedback() {
		document.querySelectorAll('form').forEach(function (form) {
			form.addEventListener('submit', function () {
				const submit = form.querySelector('button[type="submit"]');
				if (!submit) {
					return;
				}

				submit.classList.add('is-busy');
				form.classList.add('is-submitting');
				window.setTimeout(function () {
					submit.classList.remove('is-busy');
					form.classList.remove('is-submitting');
				}, 700);
			});

			form.addEventListener('invalid', function (event) {
				const target = event.target;
				if (!(target instanceof HTMLElement)) {
					return;
				}

				target.classList.remove('field-nudge');
				void target.offsetWidth;
				target.classList.add('field-nudge');
				window.setTimeout(function () {
					target.classList.remove('field-nudge');
				}, 320);
			}, true);
		});
	}

	function initDateTimePickers() {
		if (typeof window.flatpickr !== 'function') {
			return;
		}

		const browserLang = (navigator.languages && navigator.languages.length > 0
			? navigator.languages[0]
			: navigator.language) || '';
		const docLang = document.documentElement.lang || '';
		const resolvedLang = (browserLang || docLang || 'en').toLowerCase();
		const useHr = resolvedLang.startsWith('hr');
		if (useHr && window.flatpickr.l10ns && window.flatpickr.l10ns.hr) {
			window.flatpickr.localize(window.flatpickr.l10ns.hr);
		}

		document.querySelectorAll('[data-datetime-picker]').forEach(function (input) {
			const enableTime = (input.dataset.enableTime || 'true') === 'true';

			window.flatpickr(input, {
				enableTime: enableTime,
				time_24hr: true,
				dateFormat: enableTime ? 'Y-m-d H:i' : 'Y-m-d',
				altInput: true,
				altFormat: enableTime
					? (useHr ? 'd.m.Y H:i' : 'M j, Y h:i K')
					: (useHr ? 'd.m.Y' : 'M j, Y'),
				locale: useHr ? 'hr' : 'default'
			});
		});
	}

	document.addEventListener('DOMContentLoaded', function () {
		initAjaxListSearch();
		initAutocompleteDropdowns();
		initDateTimePickers();
		initFormMicroFeedback();
		document.querySelectorAll('[id$="ListTarget"]').forEach(function (target) {
			animateCards(target);
		});
	});
})();
