(function () {
  var maintenanceState = {
    config: null,
    observer: null,
    isApplying: false,
    applyRafId: 0
  };

  function normalizePath(path) {
    if (!path || typeof path !== 'string') {
      return '';
    }
    if (path.startsWith('http://') || path.startsWith('https://') || path.startsWith('#') || path.startsWith('./') || path.startsWith('../')) {
      return path;
    }
    if (path.startsWith('/')) {
      return '.' + path;
    }
    return './' + path;
  }

  function normalizeIndex(index, length) {
    return ((index % length) + length) % length;
  }

  function safeText(value, fallback) {
    if (typeof value === 'string' && value.trim() !== '') {
      return value;
    }
    return fallback;
  }

  function ensureArray(value) {
    return Array.isArray(value) ? value : [];
  }

  function hasOwn(obj, key) {
    return Object.prototype.hasOwnProperty.call(obj || {}, key);
  }

  function getDefaultMigrationConfig() {
    return {
      enabledSections: {
        mainVisual: true,
        banner: true,
        news: true,
        works: true,
        about: true,
        contact: true,
        footer: true
      }
    };
  }

  function mergeMigrationConfig(rawMigration) {
    var defaults = getDefaultMigrationConfig();
    var enabledDefaults = defaults.enabledSections;
    var enabledRaw = rawMigration && rawMigration.enabledSections && typeof rawMigration.enabledSections === 'object'
      ? rawMigration.enabledSections
      : {};

    var enabled = {};
    Object.keys(enabledDefaults).forEach(function (key) {
      enabled[key] = typeof enabledRaw[key] === 'boolean' ? enabledRaw[key] : enabledDefaults[key];
    });

    return { enabledSections: enabled };
  }

  function sanitizeConfig(raw) {
    var base = raw && typeof raw === 'object' ? raw : {};
    return {
      migration: mergeMigrationConfig(base.migration),
      mainVisualItems: ensureArray(base.mainVisualItems),
      bannerItems: ensureArray(base.bannerItems),
      newsItems: ensureArray(base.newsItems),
      works: base.works && typeof base.works === 'object' ? base.works : {},
      about: base.about && typeof base.about === 'object' ? base.about : {},
      contact: base.contact && typeof base.contact === 'object' ? base.contact : {},
      footer: base.footer && typeof base.footer === 'object' ? base.footer : {}
    };
  }

  function isSectionEnabled(config, sectionName) {
    var migration = config && config.migration;
    if (!migration || !migration.enabledSections) {
      return true;
    }

    var enabled = migration.enabledSections[sectionName];
    return typeof enabled === 'boolean' ? enabled : true;
  }

  function ensureBannerAnchor(slideInner, template) {
    var existingAnchor = slideInner.querySelector('a.Slider__Item-sc-e3sqqr-1');
    if (existingAnchor) {
      return existingAnchor;
    }

    if (template && template.content) {
      var fragment = document.importNode(template.content, true);
      slideInner.innerHTML = '';
      slideInner.appendChild(fragment);
      return slideInner.querySelector('a.Slider__Item-sc-e3sqqr-1');
    }

    var anchor = document.createElement('a');
    anchor.className = 'Slider__Item-sc-e3sqqr-1 kgEunG';
    anchor.style.width = '100%';
    anchor.style.display = 'inline-block';

    var image = document.createElement('img');
    image.className = 'Slider__Image-sc-e3sqqr-2 ZaPJE';

    anchor.appendChild(image);
    slideInner.innerHTML = '';
    slideInner.appendChild(anchor);
    return anchor;
  }

  function updateMainVisualFromConfig(mainVisualItems) {
    if (mainVisualItems.length === 0) {
      return;
    }

    var wrappers = document.querySelectorAll('.Accordion__Wrapper-sc-19fenr7-0 .Illust__Wrapper-sc-19znc2x-0');
    wrappers.forEach(function (wrapper, index) {
      var item = mainVisualItems[index];
      if (!item) {
        return;
      }

      var image = wrapper.querySelector('img');
      if (!image) {
        return;
      }

      wrapper.setAttribute('href', safeText(item.href, '#'));
      image.setAttribute('src', normalizePath(item.image));

      var title = safeText(item.title, 'main visual');
      image.setAttribute('title', title);
      image.setAttribute('alt', title);
    });
  }

  function updateBannersFromConfig(bannerItems) {
    if (bannerItems.length === 0) {
      return;
    }

    var slides = document.querySelectorAll('.Banner__Wrapper-sc-130zxj4-0 .slick-slide[data-index]');
    var template = document.getElementById('banner-item-template');

    slides.forEach(function (slide) {
      var dataIndex = parseInt(slide.getAttribute('data-index'), 10);
      if (isNaN(dataIndex)) {
        return;
      }

      var item = bannerItems[normalizeIndex(dataIndex, bannerItems.length)];
      if (!item) {
        return;
      }

      var slideInner = slide.firstElementChild;
      if (!slideInner) {
        return;
      }

      var anchor = ensureBannerAnchor(slideInner, template);
      if (!anchor) {
        return;
      }

      var image = anchor.querySelector('img.Slider__Image-sc-e3sqqr-2');
      if (!image) {
        image = document.createElement('img');
        image.className = 'Slider__Image-sc-e3sqqr-2 ZaPJE';
        anchor.appendChild(image);
      }

      anchor.setAttribute('href', safeText(item.href, '#'));
      anchor.setAttribute('target', safeText(item.target, '_blank'));
      anchor.setAttribute('tabindex', '-1');

      var title = safeText(item.title, 'banner');
      image.setAttribute('src', normalizePath(item.image));
      image.setAttribute('title', title);
      image.setAttribute('alt', title);
    });

    var dotsRoot = document.querySelector('.Banner__Wrapper-sc-130zxj4-0 .slick-dots');
    if (dotsRoot) {
      dotsRoot.innerHTML = '';
      for (var i = 0; i < bannerItems.length; i++) {
        var li = document.createElement('li');
        if (i === 0) {
          li.className = 'slick-active';
        }
        var button = document.createElement('button');
        button.textContent = String(i + 1);
        li.appendChild(button);
        dotsRoot.appendChild(li);
      }
    }
  }

  function updateNewsFromConfig(newsItems) {
    var listRoot = document.querySelector('.NewsBox__List-sc-1gifetl-8');
    if (!listRoot) {
      return;
    }

    listRoot.innerHTML = '';
    newsItems.forEach(function (item) {
      var row = document.createElement('div');
      row.className = 'NewsBox__ListItem-sc-1gifetl-9 bfuIyc';

      var dateNode = document.createElement('div');
      dateNode.className = 'NewsBox__PublishDate-sc-1gifetl-10 caBjzi';
      dateNode.textContent = safeText(item.date, '---- -- --');

      var textNode = document.createElement('div');
      textNode.className = 'NewsBox__Text-sc-1gifetl-11 iyQqtw';
      var textValue = safeText(item.text, '');
      var href = normalizePath(safeText(item.href, ''));
      var target = safeText(item.target, '_self');

      if (href) {
        var link = document.createElement('a');
        link.href = href;
        link.target = target;
        link.rel = link.target === '_blank' ? 'noopener noreferrer' : '';
        link.style.color = 'inherit';
        link.style.textDecoration = 'none';
        link.style.position = 'relative';
        link.style.zIndex = '20';
        link.style.pointerEvents = 'auto';
        link.style.cursor = 'pointer';
        link.textContent = textValue;
        textNode.appendChild(link);

        row.style.cursor = 'pointer';
        row.setAttribute('title', textValue || href);
        row.addEventListener('click', function (event) {
          if (event.target && event.target.closest && event.target.closest('a')) {
            return;
          }

          if (target === '_blank') {
            window.open(href, '_blank', 'noopener');
          } else {
            window.location.href = href;
          }
        });
      } else {
        textNode.textContent = textValue;
      }

      textNode.style.position = 'relative';
      textNode.style.zIndex = '20';
      textNode.style.pointerEvents = 'auto';

      row.appendChild(dateNode);
      row.appendChild(textNode);
      listRoot.appendChild(row);
    });
  }

  function updateWorksFromConfig(worksConfig) {
    if (!worksConfig || typeof worksConfig !== 'object') {
      return;
    }

    var moreButton = document.querySelector('#works .Button__Wrapper-sc-1wtijey-3');
    if (moreButton && moreButton.parentElement && moreButton.parentElement.tagName === 'A') {
      if (hasOwn(worksConfig, 'moreHref')) {
        moreButton.parentElement.setAttribute('href', safeText(worksConfig.moreHref, '#'));
      }
      if (hasOwn(worksConfig, 'moreTarget')) {
        moreButton.parentElement.setAttribute('target', safeText(worksConfig.moreTarget, '_blank'));
      }
    }

    var movieLink = document.querySelector('#works a.Movie__Wrapper-sc-1gx03gr-1');
    if (movieLink) {
      if (hasOwn(worksConfig, 'movieHref')) {
        movieLink.setAttribute('href', safeText(worksConfig.movieHref, '#'));
      }
      if (hasOwn(worksConfig, 'movieTarget')) {
        movieLink.setAttribute('target', safeText(worksConfig.movieTarget, '_blank'));
      }
    }
  }

  function updateAboutFromConfig(aboutConfig) {
    if (!aboutConfig || typeof aboutConfig !== 'object') {
      return;
    }

    var tabItems = document.querySelectorAll('#about .Tab__Item-sc-121kqbq-0');
    var tabLabels = ensureArray(aboutConfig.tabItems && aboutConfig.tabItems.length ? aboutConfig.tabItems : aboutConfig.tabs);
    if (tabItems.length && tabLabels.length) {
      tabItems.forEach(function (tabNode, index) {
        if (typeof tabLabels[index] === 'string') {
          tabNode.textContent = tabLabels[index];
        }
      });
    }

    var jobNode = document.querySelector('.Name__Job-sc-1v78cr4-1 span');
    if (jobNode) {
      jobNode.textContent = safeText(aboutConfig.job, jobNode.textContent || '');
    }

    var nameNode = document.querySelector('.Name__FullName-sc-1v78cr4-2 span');
    if (nameNode) {
      nameNode.textContent = safeText(aboutConfig.name, nameNode.textContent || '');
    }

    var nameJaNode = document.querySelector('.Name__NameEn-sc-1v78cr4-3 span');
    if (nameJaNode) {
      nameJaNode.textContent = safeText(aboutConfig.nameJa, nameJaNode.textContent || '');
    }

    var twitterLink = document.querySelector('.Icon__Twitter-sc-146d2sg-2');
    if (twitterLink && hasOwn(aboutConfig, 'twitterHref')) {
      var twitterHref = safeText(aboutConfig.twitterHref, '');
      if (twitterHref) {
        twitterLink.setAttribute('href', twitterHref);
        twitterLink.setAttribute('target', '_blank');
        twitterLink.setAttribute('rel', 'noopener noreferrer');
      } else {
        twitterLink.removeAttribute('href');
        twitterLink.removeAttribute('target');
        twitterLink.removeAttribute('rel');
      }
    }

    var textWrapper = document.querySelector('.Text__Wrapper-sc-430h82-0');
    if (textWrapper) {
      var descriptionLines = ensureArray(aboutConfig.descriptions)
        .map(function (line) { return String(line == null ? '' : line); })
        .filter(function (line) { return line.trim() !== ''; });

      if (!descriptionLines.length) {
        var legacyProfile = safeText(aboutConfig.profile, '');
        var legacyHistory = safeText(aboutConfig.history, '');
        if (legacyProfile) {
          descriptionLines.push(legacyProfile);
        }
        if (legacyHistory) {
          descriptionLines.push(legacyHistory);
        }
      }

      var hasMasterpieceInput = hasOwn(aboutConfig, 'masterpiece') || hasOwn(aboutConfig, 'masterpieceLabel');
      var shouldRebuildText = descriptionLines.length > 0 || hasMasterpieceInput;

      if (shouldRebuildText) {
        textWrapper.innerHTML = '';

        descriptionLines.forEach(function (line) {
          var descWrapper = document.createElement('div');
          descWrapper.className = 'Text__DescriptionWrapper-sc-430h82-1 lceUXE';
          var desc = document.createElement('div');
          desc.className = 'Text__Description-sc-430h82-2 jNjmti';
          desc.textContent = line;
          descWrapper.appendChild(desc);
          textWrapper.appendChild(descWrapper);
        });

        if (hasMasterpieceInput) {
          var masterpieceWrapper = document.createElement('div');
          masterpieceWrapper.className = 'Text__DescriptionWrapper-sc-430h82-1 lceUXE';

          var label = document.createElement('div');
          label.className = 'Text__Label-sc-430h82-3 klmmyX';
          label.textContent = safeText(aboutConfig.masterpieceLabel, '代表作');

          var masterpiece = document.createElement('div');
          masterpiece.className = 'Text__Description-sc-430h82-2 jNjmti';
          masterpiece.innerHTML = safeText(aboutConfig.masterpiece, '').replace(/\n/g, '<br />');

          masterpieceWrapper.appendChild(label);
          masterpieceWrapper.appendChild(masterpiece);
          textWrapper.appendChild(masterpieceWrapper);
        }
      }
    }

    var linksRoot = document.querySelector('.About__Links-sc-p7tifz-2');
    if (linksRoot && Array.isArray(aboutConfig.links)) {
      linksRoot.innerHTML = '';

      ensureArray(aboutConfig.links).forEach(function (item) {
        var card = document.createElement('a');
        card.className = 'LinkCard__Wrapper-sc-1p9qe2f-0 bTkTCr';
        card.href = safeText(item.href, '#');
        card.target = safeText(item.target, '_blank');

        var thumb = document.createElement('div');
        thumb.className = 'LinkCard__Thumb-sc-1p9qe2f-1 fGyOzm';
        thumb.style.backgroundImage = 'url(' + normalizePath(item.thumb) + ')';

        var serviceNameWrapper = document.createElement('div');
        serviceNameWrapper.className = 'LinkCard__ServiceNameWrapper-sc-1p9qe2f-2 djTglo';

        var serviceName = document.createElement('div');
        serviceName.className = 'LinkCard__ServiceName-sc-1p9qe2f-3 feddFM';
        serviceName.textContent = safeText(item.serviceName, 'LINK');

        var arrow = document.createElement('div');
        arrow.className = 'LinkCard__ArrowIcon-sc-1p9qe2f-4 jIOlJz';

        serviceNameWrapper.appendChild(serviceName);
        serviceNameWrapper.appendChild(arrow);
        card.appendChild(thumb);
        card.appendChild(serviceNameWrapper);
        linksRoot.appendChild(card);
      });
    }
  }

  function rebuildContactButtons(buttonsRoot, buttonItems) {
    if (!buttonsRoot) {
      return;
    }

    var anchorTemplate = buttonsRoot.querySelector('a');
    var wrapperTemplate = anchorTemplate ? anchorTemplate.querySelector('.ShadowButton__Wrapper-sc-kl8ptp-0') : null;
    var arrowTemplate = wrapperTemplate ? wrapperTemplate.querySelector('svg') : null;
    var wrapperClass = wrapperTemplate ? wrapperTemplate.className : 'ShadowButton__Wrapper-sc-kl8ptp-0 erXTOu Contact__StyledButton-sc-f2005g-4 hpHqUG';
    var textClass = 'ShadowButton__Text-sc-kl8ptp-1 eeyDzu';

    buttonsRoot.innerHTML = '';

    buttonItems.forEach(function (buttonConfig) {
      var anchor = document.createElement('a');
      anchor.href = safeText(buttonConfig.href, '#');
      anchor.target = safeText(buttonConfig.target, '_blank');

      var wrapper = document.createElement('div');
      wrapper.className = wrapperClass;

      var textNode = document.createElement('div');
      textNode.className = textClass;
      textNode.textContent = safeText(buttonConfig.text, 'お問い合わせ');

      wrapper.appendChild(textNode);
      if (arrowTemplate) {
        wrapper.appendChild(arrowTemplate.cloneNode(true));
      }

      anchor.appendChild(wrapper);
      buttonsRoot.appendChild(anchor);
    });
  }

  function updateContactFromConfig(contactConfig) {
    if (!contactConfig || typeof contactConfig !== 'object') {
      return;
    }

    var requestNode = document.querySelector('#contact .Contact__Text-sc-f2005g-5');
    if (requestNode && hasOwn(contactConfig, 'requestHtml')) {
      requestNode.innerHTML = String(contactConfig.requestHtml || '');
    }

    var buttonsRoot = document.querySelector('#contact .Contact__ButtonWrapper-sc-f2005g-3');
    rebuildContactButtons(buttonsRoot, ensureArray(contactConfig.buttons));

    var smallTextNode = document.querySelector('#contact .Contact__SmallText-sc-f2005g-6');
    if (smallTextNode && hasOwn(contactConfig, 'smallTextHtml')) {
      smallTextNode.innerHTML = String(contactConfig.smallTextHtml || '');
    }
  }

  function updateFooterFromConfig(footerConfig) {
    if (!footerConfig || typeof footerConfig !== 'object') {
      return;
    }

    var copyrightNode = document.querySelector('.Footer__Copyright-sc-zp1huk-3');
    if (copyrightNode && hasOwn(footerConfig, 'copyright')) {
      copyrightNode.textContent = String(footerConfig.copyright || '');
    }
  }

  function runSectionUpdate(sectionName, updateFn) {
    try {
      updateFn();
    } catch (error) {
      window.__SITE_MAINTENANCE_LAST_ERROR__ = {
        section: sectionName,
        message: error && error.message ? error.message : String(error)
      };
      if (window.console && typeof window.console.warn === 'function') {
        console.warn('[site-maintenance] section update failed:', sectionName, error);
      }
    }
  }

  function applyMaintenanceConfig(config) {
    if (!config || typeof config !== 'object') {
      return;
    }

    if (isSectionEnabled(config, 'mainVisual')) {
      runSectionUpdate('mainVisual', function () {
        updateMainVisualFromConfig(config.mainVisualItems);
      });
    }
    if (isSectionEnabled(config, 'banner')) {
      runSectionUpdate('banner', function () {
        updateBannersFromConfig(config.bannerItems);
      });
    }
    if (isSectionEnabled(config, 'news')) {
      runSectionUpdate('news', function () {
        updateNewsFromConfig(config.newsItems);
      });
    }
    if (isSectionEnabled(config, 'works')) {
      runSectionUpdate('works', function () {
        updateWorksFromConfig(config.works);
      });
    }
    if (isSectionEnabled(config, 'about')) {
      runSectionUpdate('about', function () {
        updateAboutFromConfig(config.about);
      });
    }
    if (isSectionEnabled(config, 'contact')) {
      runSectionUpdate('contact', function () {
        updateContactFromConfig(config.contact);
      });
    }
    if (isSectionEnabled(config, 'footer')) {
      runSectionUpdate('footer', function () {
        updateFooterFromConfig(config.footer);
      });
    }

    document.documentElement.setAttribute('data-maintenance-source', 'maintenance/site-config.json');
  }

  function scheduleApplyFromState() {
    if (!maintenanceState.config || maintenanceState.isApplying || maintenanceState.applyRafId) {
      return;
    }

    maintenanceState.applyRafId = window.requestAnimationFrame(function () {
      maintenanceState.applyRafId = 0;
      maintenanceState.isApplying = true;
      try {
        applyMaintenanceConfig(maintenanceState.config);
      } finally {
        maintenanceState.isApplying = false;
      }
    });
  }

  function ensureObserver() {
    if (maintenanceState.observer) {
      return;
    }

    var target = document.getElementById('__next') || document.body;
    if (!target) {
      return;
    }

    maintenanceState.observer = new MutationObserver(function () {
      scheduleApplyFromState();
    });

    maintenanceState.observer.observe(target, { childList: true, subtree: true });
  }

  function loadConfigAndApply() {
    fetch('./maintenance/site-config.json', { cache: 'no-store' })
      .then(function (response) {
        if (!response.ok) {
          throw new Error('Config load failed');
        }
        return response.json();
      })
      .then(function (config) {
        maintenanceState.config = sanitizeConfig(config);
        window.__SITE_MAINTENANCE_CONFIG__ = maintenanceState.config;
        scheduleApplyFromState();
        ensureObserver();
      })
      .catch(function (error) {
        window.__SITE_MAINTENANCE_LAST_ERROR__ = {
          section: 'config-load',
          message: error && error.message ? error.message : String(error)
        };
        if (window.console && typeof window.console.warn === 'function') {
          console.warn('[site-maintenance] config load failed:', error);
        }
      });
  }

  if (document.readyState === 'loading') {
    document.addEventListener('DOMContentLoaded', loadConfigAndApply);
  } else {
    loadConfigAndApply();
  }

  window.addEventListener('load', function () {
    scheduleApplyFromState();
    ensureObserver();
  });

  window.addEventListener('site-config:refresh', loadConfigAndApply);
})();
