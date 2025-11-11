# Language Options
- [EN](#unity-procedural-terrain-generation)
- [TR](#unity-prosedürel-arazi-üretimi)

---

# Unity Procedural Terrain Generation

This repository is an extended, object-oriented and customizable version of  
**Penny de Byl’s "Procedural Terrain Generation with Unity" (Holistic3D/Udemy)** course.

It expands the original concepts with:
- cleaner OOP architecture,
- modular systems,
- custom inspectors,
- erosion models,
- splat mapping,
- vegetation/detail scattering,
- water placement,
- simple cloud generation,
- and a unified tooling workflow in URP.

## Features

### Terrain Generation
- Perlin (single, fractal, multi-layer, ridge)
- Midpoint Displacement
- Voronoi (multi-peak, realistic, hybrid)
- Random height fill
- Heightmap import

### Post-Processing
- Multi-iteration smoothing
- Erosion: Rain, Thermal, Tidal, River, Wind

### Texturing & Details
- Splatmaps via TerrainLayers (noise, slope, height rules)
- Vegetation with prototype selection and distribution rules
- Detail meshes/billboards with noise/height/slope constraints
- One-click water plane placement

### Editor Tooling
- Custom inspectors
- Foldouts, buttons, table editors
- Progress bars for heavy tasks

### Sky / Clouds
- Particle-based cloud controller
- Color/lining/speed/distance control

## Requirements
- Unity (URP recommended)
- EditorGUITable (used for table views)
  
---

# Unity Prosedürel Arazi Üretimi

Bu proje,  
**Penny de Byl’in “Procedural Terrain Generation with Unity” (Holistic3D/Udemy)** kursunun  
daha nesne yönelimli, genişletilmiş ve özelleştirilebilir bir sürümüdür.

Orijinal konseptler;  
modern OOP mimarisi, modüler sistemler, özel editor panelleri, gelişmiş erozyon, splatmap,  
bitki/detay dağıtımı, su yerleşimi ve basit bulut sistemi ile genişletilmiştir.

## Özellikler

### Arazi Üretimi
- Perlin (tek, fraktal, çok katmanlı, ridge)
- Midpoint displacement
- Voronoi (çok tepeli, gerçekçi, hibrit)
- Rastgele yükseklik doldurma
- Texture’dan heightmap import

### Son İşleme
- Çok iterasyonlu yumuşatma
- Erozyon: Yağmur, Termal, Gelgit, Nehir, Rüzgar

### Kaplama & Detay
- TerrainLayer ile splatmap (yükseklik/eğim/gürültü kuralları)
- Bitkilendirme (ağaç prototipleri, yoğunluk/ölçek/eğim/yükseklik)
- Detay mesh/billboard (yoğunluk, feather/overlap, yükseklik/eğim)
- Tek tıkla su yerleşimi

### Editor Araçları
- Özel inspector panelleri
- Parametre tabloları (EditorGUITable)
- Uzun işlemlerde progress bar

### Gökyüzü / Bulut
- Parçacık tabanlı bulut sistemi
- Renk, lining, hız ve mesafe kontrolü

## Gereksinimler
- Unity (URP tavsiye edilir)
- EditorGUITable (opsiyonel)
